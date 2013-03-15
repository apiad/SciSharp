using System;
using System.Collections.Generic;


namespace SciSharp.GameTheory
{
    public class GameTree<TGame, TBoard, TPlay> : IEquatable<GameTree<TGame, TBoard, TPlay>>
        where TGame : IGame<TBoard, TPlay>
        where TPlay : IPlay<TBoard>
    {
        private readonly HashSet<TPlay> availablePlays;
        private readonly HashSet<GameTree<TGame, TBoard, TPlay>> children;
        private readonly IGameKey key;

        public GameTree(TGame game)
            : this(default(TPlay), game) {}

        public GameTree(TPlay play, TGame game)
        {
            Play = play;

            Player = game.CurrentPlayer;

            key = game.GetKey();
            children = new HashSet<GameTree<TGame, TBoard, TPlay>>();
            availablePlays = new HashSet<TPlay>(game.ValidMoves());
        }

        public bool Completed { get; set; }

        public double MinimaxValue { get; set; }

        public double TotalPayoff { get; set; }

        public int Wins { get; set; }

        public int Plays { get; set; }

        public int Player { get; set; }

        public TPlay Play { get; set; }

        public ICollection<GameTree<TGame, TBoard, TPlay>> Children
        {
            get { return children; }
        }

        public ICollection<TPlay> AvailablePlays
        {
            get { return availablePlays; }
        }

        public IGameKey Key
        {
            get { return key; }
        }

        #region IEquatable<GameTree<TGame,TBoard,TPlay>> Members

        public bool Equals(GameTree<TGame, TBoard, TPlay> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(key, other.key);
        }

        #endregion

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((GameTree<TGame, TBoard, TPlay>) obj);
        }

        public override int GetHashCode()
        {
            return (key != null ? key.GetHashCode() : 0);
        }

        public static bool operator ==(GameTree<TGame, TBoard, TPlay> left, GameTree<TGame, TBoard, TPlay> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(GameTree<TGame, TBoard, TPlay> left, GameTree<TGame, TBoard, TPlay> right)
        {
            return !Equals(left, right);
        }
    }
}
