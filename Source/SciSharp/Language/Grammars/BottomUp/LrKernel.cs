using System.Collections.Generic;


namespace SciSharp.Language.Grammars.BottomUp
{
    public class LrKernel<T> : IEqualityComparer<LrItem<T>>
        where T : Node, new()
    {
        private readonly HashSet<LrItem<T>> items;

        public LrKernel(LrItem<T>[] items)
        {
            this.items = new HashSet<LrItem<T>>(items, this);
        }

        #region IEqualityComparer<LrItem<T>> Members

        public bool Equals(LrItem<T> x, LrItem<T> y)
        {
            return x.TotalEquals(y);
        }

        public int GetHashCode(LrItem<T> obj)
        {
            return obj.GetHashCode();
        }

        #endregion

        public override bool Equals(object obj)
        {
            return (obj is LrKernel<T>) &&
                   Equals((LrKernel<T>) obj);
        }

        public bool Equals(LrKernel<T> other)
        {
            if (items.Count != other.items.Count)
                return false;

            foreach (var lrItem in items)
            {
                if (!other.items.Contains(lrItem))
                    return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            int ghc = 397;

            foreach (var lrItem in items)
                ghc ^= lrItem.GetHashCode();

            return ghc;
        }
    }
}
