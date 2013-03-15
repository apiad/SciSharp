using System;

using SciSharp.Collections;


namespace SciSharp.Sorting
{
    public class HeapSort<T> : Sorter<T>
    {
        protected override void SafeSort(T[] items, int start, int end, Comparison<T> comparison)
        {
            var heap = new BinaryHeap<T>(end - start + 1, comparison);

            for (int i = start; i <= end; i++)
                heap.Add(items[i]);

            while (heap.Count > 0)
                items[start++] = heap.Extract();
        }
    }
}
