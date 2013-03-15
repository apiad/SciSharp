using System;


namespace SciSharp.Language.Grammars.BottomUp
{
    public class SlrItem<T> : IEquatable<SlrItem<T>>
        where T : Node, new()
    {
        public Def<T> Symbol { get; set; }
        public ProductionRule<T> Body { get; set; }
        public int Pointer { get; set; }

        #region IEquatable<SlrItem<T>> Members

        public bool Equals(SlrItem<T> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Symbol, Symbol) && Equals(other.Body, Body) && other.Pointer == Pointer;
        }

        #endregion

        public SlrItem<T> Next()
        {
            if (Pointer == Body.Defs.Count)
                throw new ArgumentException("The pointer is already at the end.");

            return new SlrItem<T>
                   {
                       Pointer = Pointer + 1,
                       Symbol = Symbol,
                       Body = Body
                   };
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (SlrItem<T>)) return false;
            return Equals((SlrItem<T>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = (Symbol != null ? Symbol.GetHashCode() : 0);
                result = (result*397) ^ (Body != null ? Body.GetHashCode() : 0);
                result = (result*397) ^ Pointer;
                return result;
            }
        }

        public static bool operator ==(SlrItem<T> left, SlrItem<T> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(SlrItem<T> left, SlrItem<T> right)
        {
            return !Equals(left, right);
        }
    }
}
