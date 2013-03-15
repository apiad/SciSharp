using System;

using SciSharp.Collections;


namespace SciSharp.Sorting
{
    public class MinimumSort<T> : Sorter<T>
    {
        protected override void SafeSort(T[] items, int start, int end, Comparison<T> comparison)
        {
            while (start < end)
                items.Swap(start++, FindMinimum(items, start, end, comparison));
        }

        private int FindMinimum(T[] items, int start, int end, Comparison<T> comparison)
        {
            int minPos = start;

            while (start < end)
            {
                if (comparison(items[start], items[minPos]) < 0)
                    minPos = start;

                start++;
            }

            return minPos;
        }
    }
}
