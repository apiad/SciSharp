using System;


namespace SciSharp.Collections
{
    public class ArrayStack<T> : IStack<T>
    {
        private readonly int capacity;
        private T[] items;
        private int count;

        public ArrayStack() : this(32) { }

        public ArrayStack(int capacity)
        {
            this.capacity = capacity;
            items = new T[capacity];
        }

        public T this[int index]
        {
            get
            {
                if (index < 0)
                    index = count + index;

                if (index < 0 || index >= count)
                    throw new IndexOutOfRangeException("Must fall in the interval [0, count).");

                return items[index];
            }
        }

        public int Count
        {
            get { return count; }
        }

        public void Push(T item)
        {
            if (count == items.Length)
                Grow();

            items[count++] = item;
        }

        private void Grow()
        {
            var temp = new T[items.Length * 2];
            Array.Copy(items, temp, count);
            items = temp;
        }

        public T Peek()
        {
            if (count == 0)
                throw new InvalidOperationException("The stack is empty.");

            return items[0];
        }

        public T Pop()
        {
            if (count == 0)
                throw new InvalidOperationException("The stack is empty.");

            return items[--count];
        }

        public void Clear()
        {
            count = 0;
            items = new T[capacity];
        }
    }
}