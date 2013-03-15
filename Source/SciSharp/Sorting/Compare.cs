using System;
using System.Collections.Generic;


namespace SciSharp.Sorting
{
    public static class Compare
    {
        public static int Default<T>(T x, T y)
        {
            return Comparer<T>.Default.Compare(x, y);
        }

        public static int Inverse<T>(T x, T y)
        {
            return Comparer<T>.Default.Compare(y, x);
        }

        public static Comparison<T> ByKey<T, TKey>(Func<T, TKey> key)
            where TKey : IComparable<TKey>
        {
            return (x, y) => key(x).CompareTo(key(y));
        }
    }
}
