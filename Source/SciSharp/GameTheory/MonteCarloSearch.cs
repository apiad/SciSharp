using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace SciSharp.GameTheory
{
    public class MonteCarloSearch<TGame, TBoard, TPlay>
        where TGame : IGame<TBoard, TPlay>
        where TPlay : IPlay<TBoard>
    {
        private readonly bool deterministic;
        private readonly int maxTreeSize;
        private readonly int player;
        private readonly MonteCarloPlayout<TPlay> playoutStrategy;
        private readonly MonteCarloSelection<TGame, TBoard, TPlay> selectionStrategy;
        //private int currentTime;
        private TGame gameInstance;

        private GameTree<TGame, TBoard, TPlay> root;

        public MonteCarloSearch(int maxTreeSize, TGame game, int player, bool deterministic,
                                MonteCarloSelection<TGame, TBoard, TPlay> selectionStrategy,
                                MonteCarloPlayout<TPlay> playoutStrategy)
        {
            this.maxTreeSize = maxTreeSize;
            gameInstance = game;
            this.player = player;
            this.deterministic = deterministic;
            this.selectionStrategy = selectionStrategy;
            this.playoutStrategy = playoutStrategy;

            root = new GameTree<TGame, TBoard, TPlay>(game);
        }

        public GameTree<TGame, TBoard, TPlay> Tree
        {
            get { return root; }
        }

        public void Expand(int iterations)
        {
            // Expand until we run out of iterations or the tree is completed
            while (iterations-- > 0 && !root.Completed)
                Expand();
        }

        public void ExpandParallel(int time, Func<TGame, TGame> gameFactory)
        {
            int start = Environment.TickCount;

            // Creating all of root's children
            while (root.AvailablePlays.Count > 0)
                Expand(root, gameInstance);

            // Create the children games
            TGame[] games = root.Children.Select(c =>
                                                 {
                                                     TGame g = gameFactory(gameInstance);
                                                     c.Play.Apply(g.Board);
                                                     return g;
                                                 }).ToArray();

            // Create a task to expand each child
            Parallel.ForEach(root.Children, (tree, state, i) =>
                                            {
                                                TGame game = games[i];

                                                // Make sure we don't waste time
                                                while (Environment.TickCount - start < time && !tree.Completed)
                                                    Expand(tree, game);
                                            });

            // Update (reset) the root data
            root.Wins = root.Children.Sum(c => c.Wins);
            root.Plays = root.Children.Sum(c => c.Plays);
            root.TotalPayoff = root.Children.Sum(c => c.TotalPayoff);
        }

        public void Expand()
        {
            Expand(root, gameInstance);
        }

        public void UpdateRoot(TGame newGame)
        {
            // Update the current game
            gameInstance = newGame;

            // Compute the game's new key
            IGameKey newKey = newGame.GetKey();

            // If the game hasn't changed, do nothing
            if (Equals(root.Key, newKey))
                return;

            // Get the child that has this game as root
            GameTree<TGame, TBoard, TPlay> child = root.Children.FirstOrDefault(c => Equals(c.Key, newKey));

            // Set the root to this child, or a new tree if necessary
            root = child ?? new GameTree<TGame, TBoard, TPlay>(gameInstance);

            // Report the save nodes
            Logger.Log("Updated root. Kept {0} games", root.Plays);
        }

        private void Expand(GameTree<TGame, TBoard, TPlay> tree, TGame game)
        {
            // Check terminal condition and update 
            if (Terminal(tree, game))
                return;

            GameTree<TGame, TBoard, TPlay> child;

            // Only explore on the fringe
            if (tree.AvailablePlays.Count > 0)
            {
                // Select the next play to expand
                TPlay play = tree.AvailablePlays.ArgMax(p => playoutStrategy(p));

                // Apply the play to change the state of the game
                play.Apply(game.Board);

                // Create the new child
                child = new GameTree<TGame, TBoard, TPlay>(play, game);

                // Add the child if we have space
                tree.Children.Add(child);
                tree.AvailablePlays.Remove(play);

                // Playout
                Playout(child, game);

                // Backtracking
                play.Undo(game.Board);
            }
            else
            {
                // Select the child to exploit
                GameTree<TGame, TBoard, TPlay>[] children = tree.Children.Where(c => !c.Completed).ToArray();

                // If all children are completed, this tree is completed
                if (children.Length == 0)
                {
                    tree.Completed = true;

                    // Calculate the minimax value
                    tree.MinimaxValue = MinimaxValue(game.CurrentPlayer, tree.Children);

                    // Nothing else to do here
                    return;
                }

                child = children.ArgMax(c => selectionStrategy(tree, c));

                // Remove the child data from the parent, for later update
                tree.Wins -= child.Wins;
                tree.TotalPayoff -= child.TotalPayoff;
                tree.Plays -= child.Plays;

                // Expand
                child.Play.Apply(game.Board);
                Expand(child, game);
                child.Play.Undo(game.Board);
            }

            // Update the tree data
            Update(tree, child);
        }

        private double MinimaxValue(int currentPlayer, IEnumerable<GameTree<TGame, TBoard, TPlay>> children)
        {
            if (currentPlayer == player)
                return children.Max(c => c.MinimaxValue);

            return children.Min(c => c.MinimaxValue);
        }

        private static void Update(GameTree<TGame, TBoard, TPlay> tree, GameTree<TGame, TBoard, TPlay> child)
        {
            // Update the tree data
            tree.Wins += child.Wins;
            tree.TotalPayoff += child.TotalPayoff;
            tree.Plays += child.Plays;
        }

        private bool Terminal(GameTree<TGame, TBoard, TPlay> tree, TGame game)
        {
            // Already completed
            if (tree.Completed)
                return true;

            // Is not a terminal state
            if (!game.Finished)
                return false;

            // Calculate the game's output
            double payoff = game.Payoff(player);

            // Update the leaf data
            tree.Wins += (payoff > 0) ? 1 : 0;
            tree.TotalPayoff += payoff;
            tree.Plays++;

            // Mark as completed
            if (deterministic)
            {
                tree.Completed = true;

                // Set the minimax value
                tree.MinimaxValue = payoff;
            }

            // Nothing else to do here
            return true;
        }

        private void Playout(GameTree<TGame, TBoard, TPlay> tree, TGame game)
        {
            // Check terminal condition and update
            if (Terminal(tree, game))
                return;

            // Select the next play
            TPlay play = tree.AvailablePlays.ArgMax(p => playoutStrategy(p));

            // Create the new child (will not be stored on the tree)
            play.Apply(game.Board);
            var child = new GameTree<TGame, TBoard, TPlay>(play, game);

            // Playout
            Playout(child, game);
            play.Undo(game.Board);

            // Update the tree data
            Update(tree, child);
        }

        public void UpdateRoot(TPlay play)
        {
            play.Apply(gameInstance.Board);
            UpdateRoot(gameInstance);
        }
    }
}
