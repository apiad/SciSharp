using System;
using System.Collections.Generic;


namespace SciSharp
{
    public class KeyComparer<T, TKey> : IComparer<T>
        where TKey : IComparable<TKey>
    {
        private readonly Func<T, TKey> key;

        public KeyComparer(Func<T, TKey> key)
        {
            this.key = key;
        }

        #region IComparer<T> Members

        public int Compare(T x, T y)
        {
            return key(x).CompareTo(key(y));
        }

        #endregion
    }
}
