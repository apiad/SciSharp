using System;


namespace SciSharp.Language.Grammars.BottomUp
{
    [Serializable]
    public struct ActionKey<T> : IEquatable<ActionKey<T>>
        where T : Node, new()
    {
        private readonly int state;
        private readonly Token<T> symbol;

        public ActionKey(int state, Token<T> symbol)
        {
            this.state = state;
            this.symbol = symbol;
        }

        public int State
        {
            get { return state; }
        }

        public Token<T> Symbol
        {
            get { return symbol; }
        }

        #region IEquatable<ActionKey<T>> Members

        public bool Equals(ActionKey<T> other)
        {
            return other.state == state && Equals(other.symbol, symbol);
        }

        #endregion

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (obj.GetType() != typeof (ActionKey<T>))
                return false;
            return Equals((ActionKey<T>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (state*397) ^ (symbol != null ? symbol.GetHashCode() : 0);
            }
        }

        public static bool operator ==(ActionKey<T> left, ActionKey<T> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ActionKey<T> left, ActionKey<T> right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return string.Format("State: {0}, Symbol: {1}", state, symbol);
        }
    }
}
