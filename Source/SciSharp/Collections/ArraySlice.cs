using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace SciSharp.Collections
{
    public class ArraySlice<T> : IFiniteSet<T>
    {
        #region Instance Fields

        private readonly int end;
        private readonly T[] items;
        private readonly int start;
        private readonly int step;

        #endregion

        #region Constructors

        public ArraySlice(T[] items)
            : this(items, 0, items.Length - 1) {}

        public ArraySlice(T[] items, int start, int end)
            : this(items, start, 1, end) {}

        public ArraySlice(T[] items, int start, int step, int end)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            if (step == 0)
                throw new ArgumentOutOfRangeException("step");

            if (start < 0)
                start = items.Length + start;

            if (end < 0)
                end = items.Length + end;

            if (start < 0 || start > items.Length - 1)
                throw new ArgumentOutOfRangeException("start");

            if (end < 0 || end > items.Length - 1)
                throw new ArgumentException("end");

            this.items = items;
            this.start = start;
            this.step = step;
            this.end = end;
        }

        #endregion

        #region Properties

        public T this[int index]
        {
            get
            {
                if (index < 0)
                    index = Count + index;

                if (index < 0 || index >= Count)
                    throw new ArgumentOutOfRangeException("index");

                return items[start + index*step];
            }
            set
            {
                if (index < 0)
                    index = Count + index;

                if (index < 0 || index >= Count)
                    throw new ArgumentOutOfRangeException("index");

                items[start + index*step] = value;
            }
        }

        public ArraySlice<T> this[int start, int end]
        {
            get { return this[start, 1, end]; }
            set { this[start, 1, end] = value; }
        }

        public ArraySlice<T> this[int start, int step, int end]
        {
            get
            {
                int newStart = start + this.start;
                int newEnd = newStart + step*(end - start);
                int newStep = step*this.step;

                return new ArraySlice<T>(items, newStart, newStep, newEnd);
            }
            set
            {
                int newStart = start + this.start;
                int newEnd = end + this.end;
                int newStep = step*this.step;

                var slice = new ArraySlice<T>(items, newStart, newStep, newEnd);
                ArraySlice<T> other = value;

                for (int i = 0; i < slice.Count && i < other.Count; i++)
                    slice[i] = other[i];
            }
        }

        #endregion

        #region IFiniteSet<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
                yield return this[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Contains(T item)
        {
            return ((IEnumerable<T>) this).Contains(item);
        }

        public int Count
        {
            get
            {
                int length = (end - start + 1);
                int count = length/step + (length%step > 0 ? 1 : 0);
                return count > 0 ? count : 0;
            }
        }

        public T[] ToArray()
        {
            var array = new T[end];
            int i = 0;

            foreach (T x in this)
                array[i++] = x;

            return array;
        }

        #endregion
    }
}
