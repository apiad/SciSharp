using System;

using SciSharp.Collections;


namespace SciSharp.Sorting
{
    public class QuickSort<T> : Sorter<T>
    {
        protected override void SafeSort(T[] items, int start, int end, Comparison<T> comparison)
        {
            T mid = items[(start + end) >> 1];
            int l = start;
            int r = end;

            do
            {
                while (comparison(items[l], mid) < 0)
                    l++;

                while (comparison(items[r], mid) > 0)
                    r--;

                if (l <= r)
                    items.Swap(l++, r--);
            } while (l <= r);

            if (l < end)
                SafeSort(items, l, end, comparison);

            if (r > start)
                SafeSort(items, start, r, comparison);
        }
    }
}
