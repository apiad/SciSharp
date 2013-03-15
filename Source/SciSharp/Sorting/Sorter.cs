using System;


namespace SciSharp.Sorting
{
    public abstract class Sorter<T> : ISorter<T>
    {
        #region ISorter<T> Members

        public void Sort(T[] items, int start, int end, Comparison<T> comparison)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            if (comparison == null)
                throw new ArgumentNullException("comparison");

            if (start < 0 || end >= items.Length)
                throw new ArgumentOutOfRangeException();

            if (start < end)
                SafeSort(items, start, end, comparison);
        }

        #endregion

        protected abstract void SafeSort(T[] items, int start, int end, Comparison<T> comparison);
    }
}
