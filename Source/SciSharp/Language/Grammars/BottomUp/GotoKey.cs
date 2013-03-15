using System;


namespace SciSharp.Language.Grammars.BottomUp
{
    [Serializable]
    public struct GotoKey<T> : IEquatable<GotoKey<T>>
        where T : Node, new()
    {
        private readonly int state;
        private readonly Def<T> symbol;

        public GotoKey(int state, Def<T> symbol)
        {
            this.state = state;
            this.symbol = symbol;
        }

        public int State
        {
            get { return state; }
        }

        public Def<T> Symbol
        {
            get { return symbol; }
        }

        #region IEquatable<GotoKey<T>> Members

        public bool Equals(GotoKey<T> other)
        {
            return other.state == state && Equals(other.symbol, symbol);
        }

        #endregion

        public static bool operator ==(GotoKey<T> left, GotoKey<T> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(GotoKey<T> left, GotoKey<T> right)
        {
            return !left.Equals(right);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (obj.GetType() != typeof (GotoKey<T>))
                return false;
            return Equals((GotoKey<T>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (state*397) ^ (symbol != null ? symbol.GetHashCode() : 0);
            }
        }

        public override string ToString()
        {
            return string.Format("State: {0}, Symbol: {1}", state, symbol);
        }
    }
}
