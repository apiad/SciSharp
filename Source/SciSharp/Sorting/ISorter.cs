using System;


namespace SciSharp.Sorting
{
    public interface ISorter<T>
    {
        void Sort(T[] items, int start, int end, Comparison<T> comparison);
    }
}
