using System;


namespace SciSharp.Sorting
{
    public class MergeSort<T> : Sorter<T>
    {
        protected override void SafeSort(T[] items, int start, int end, Comparison<T> comparison)
        {
            if (start == end)
                return;

            int mid = (start + end) >> 1;

            SafeSort(items, start, mid, comparison);
            SafeSort(items, mid + 1, end, comparison);

            Merge(items, start, mid, end, comparison);
        }

        private void Merge(T[] items, int start, int mid, int end, Comparison<T> comparison)
        {
            var temp = new T[end - start + 1];
            int p = 0;
            int l = start;
            int r = mid + 1;

            while (l < mid && r < end)
            {
                if (comparison(items[l], items[r]) <= 0)
                    temp[p++] = items[l++];
                else
                    temp[p++] = items[r++];
            }

            while (l < mid)
                temp[p++] = items[l++];

            while (r < end)
                temp[p++] = items[r++];

            Array.Copy(temp, 0, items, start, temp.Length);
        }
    }
}
