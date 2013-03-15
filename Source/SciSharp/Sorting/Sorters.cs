using System;
using System.Collections.Generic;


namespace SciSharp.Sorting
{
    public static class Sorters
    {
        #region Members

        public static void Sort<T, TKey>(this ISorter<T> sorter, T[] items, Func<T, TKey> key)
            where TKey : IComparable<TKey>
        {
            Sort(sorter, items, 0, items.Length, key);
        }

        public static void Sort<T, TKey>(this ISorter<T> sorter, T[] items, int start, Func<T, TKey> key)
            where TKey : IComparable<TKey>
        {
            Sort(sorter, items, start, items.Length - start, key);
        }

        public static void Sort<T, TKey>(this ISorter<T> sorter, T[] items, int start, int length, Func<T, TKey> key)
            where TKey : IComparable<TKey>
        {
            sorter.Sort(items, start, length, (x, y) => key(x).CompareTo(key(y)));
        }

        public static void Sort<T>(this ISorter<T> sorter, T[] items, Comparison<T> comparison)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            sorter.Sort(items, 0, items.Length, comparison);
        }

        public static void Sort<T>(this ISorter<T> sorter, T[] items, int start, Comparison<T> comparison)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            sorter.Sort(items, start, items.Length - start, comparison);
        }

        public static void Sort<T>(this ISorter<T> sorter, T[] items, IComparer<T> comparer)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            sorter.Sort(items, 0, items.Length, comparer);
        }

        public static void Sort<T>(this ISorter<T> sorter, T[] items, int start, IComparer<T> comparer)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            sorter.Sort(items, start, items.Length - start, comparer);
        }

        public static void Sort<T>(this ISorter<T> sorter, T[] items, int start, int length, IComparer<T> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            sorter.Sort(items, start, length, comparer.Compare);
        }

        public static void Sort<T>(this ISorter<T> sorter, T[] items)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            sorter.Sort(items, 0, items.Length);
        }

        public static void Sort<T>(this ISorter<T> sorter, T[] items, int start)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            sorter.Sort(items, start, items.Length - start);
        }

        public static void Sort<T>(this ISorter<T> sorter, T[] items, int start, int length)
        {
            sorter.Sort(items, start, length, Comparer<T>.Default);
        }

        #endregion
    }
}
