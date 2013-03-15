using System;
using System.Collections;
using System.Collections.Generic;


namespace SciSharp.Collections
{
    public class BinaryHeap<T> : IQueue<T>, IEnumerable<T>
    {
        private readonly Comparison<T> comparison;
        private readonly int size;
        private int end;
        private T[] items;

        public BinaryHeap()
            : this(1024)
        {
            comparison = Comparer<T>.Default.Compare;
        }

        public BinaryHeap(Comparison<T> comparison)
            : this(1024, comparison) {}

        public BinaryHeap(int size, Comparison<T> comparison)
            : this(size)
        {
            if (comparison == null)
                throw new ArgumentNullException("comparison");

            this.comparison = comparison;
        }

        public BinaryHeap(int size)
        {
            this.size = size;
            items = new T[size + 1];
            end = 1;

            comparison = Comparer<T>.Default.Compare;
        }

        #region Implementation of IQueue<T>

        void IQueue<T>.Enqueue(T item)
        {
            Add(item);
        }

        T IQueue<T>.Peek()
        {
            return Min;
        }

        T IQueue<T>.Dequeue()
        {
            return Extract();
        }

        #endregion

        public T Min
        {
            get
            {
                if (Count == 0)
                    throw new InvalidOperationException("The heap is empty.");

                return items[1];
            }
        }

        public bool Empty
        {
            get { return Count == 0; }
        }

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 1; i < end; i++)
            {
                yield return items[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region IQueue<T> Members

        public int Count
        {
            get { return end - 1; }
        }

        #endregion

        private static int LeftChild(int i)
        {
            return i << 1;
        }

        private static int RightChild(int i)
        {
            return (i << 1) + 1;
        }

        private static int Parent(int i)
        {
            return i >> 1;
        }

        public void Add(T item)
        {
            Grow();
            items[end++] = item;
            HeapifyUp(Count);
        }

        public T Extract()
        {
            T result = Min;

            items[1] = items[Count];
            items[Count] = default(T);
            end--;

            if (Count > 0)
                HeapifyDown(1);

            return result;
        }

        private void HeapifyUp(int i)
        {
            int p = Parent(i);

            while (i > 1 && comparison(items[p], items[i]) > 0)
            {
                Swap(i, p);

                i = p;
                p = Parent(i);
            }
        }

        private void Swap(int i, int j)
        {
            // Swaping items
            T temp = items[j];
            items[j] = items[i];
            items[i] = temp;
        }

        private void HeapifyDown(int i)
        {
            while (i <= Count)
            {
                int left = LeftChild(i);
                int right = RightChild(i);
                int min = i;

                if (left < end && comparison(items[left], items[i]) < 0)
                    min = left;
                if (right < end && comparison(items[right], items[min]) < 0)
                    min = right;

                if (min == i)
                    break;

                Swap(i, min);
                i = min;
            }
        }

        private void Grow()
        {
            if (end == items.Length)
            {
                var temp = new T[items.Length + size];
                Array.Copy(items, temp, items.Length);
                items = temp;
            }
        }
    }
}
