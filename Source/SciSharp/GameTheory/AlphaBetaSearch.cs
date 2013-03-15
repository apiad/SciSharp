using System;


namespace SciSharp.GameTheory
{
    public class AlphaBetaSearch<TGame, TBoard, TPlay>
        where TGame : IGame<TBoard, TPlay>
        where TPlay : IPlay<TBoard>
    {
        private readonly Func<TGame, int, double> heuristicPayoff;
        private readonly double payoffBias;

        public AlphaBetaSearch(Func<TGame, int, double> heuristicPayoff, double payoffBias = 1d)
        {
            this.heuristicPayoff = heuristicPayoff;
            this.payoffBias = payoffBias;
        }

        public TPlay BestMove(TGame game, int player, int depth)
        {
            return Max(game, player, depth, double.NegativeInfinity, double.PositiveInfinity).Item1;
        }

        protected Tuple<TPlay, double> Max(TGame game, int player, int depth, double alpha, double beta)
        {
            double v = double.NegativeInfinity;
            TPlay best = default(TPlay);

            foreach (TPlay play in game.ValidMoves())
            {
                play.Apply(game.Board);

                int nextPlayer = game.CurrentPlayer;

                double value = game.Finished
                                   ? game.Payoff(player)*payoffBias
                                   : depth <= 0
                                         ? heuristicPayoff(game, player)
                                         : nextPlayer == player
                                               ? Max(game, nextPlayer, depth - 1, alpha, beta).Item2
                                               : Min(game, nextPlayer, depth - 1, alpha, beta).Item2;

                if (value > v)
                {
                    v = value;
                    best = play;
                }

                play.Undo(game.Board);

                if (v >= beta)
                    return Tuple.Create(best, v);

                alpha = Math.Max(alpha, v);
            }

            return Tuple.Create(best, v);
        }

        protected Tuple<TPlay, double> Min(TGame game, int player, int depth, double alpha, double beta)
        {
            double v = double.PositiveInfinity;
            TPlay best = default(TPlay);

            foreach (TPlay play in game.ValidMoves())
            {
                play.Apply(game.Board);

                int nextPlayer = game.CurrentPlayer;

                double value = game.Finished
                                   ? game.Payoff(player)*payoffBias
                                   : depth <= 0
                                         ? heuristicPayoff(game, player)
                                         : nextPlayer == player
                                               ? Min(game, nextPlayer, depth - 1, alpha, beta).Item2
                                               : Max(game, nextPlayer, depth - 1, alpha, beta).Item2;

                if (value < v)
                {
                    v = value;
                    best = play;
                }

                play.Undo(game.Board);

                if (v <= alpha)
                    return Tuple.Create(best, v);

                beta = Math.Min(beta, v);
            }

            return Tuple.Create(best, v);
        }
    }
}
