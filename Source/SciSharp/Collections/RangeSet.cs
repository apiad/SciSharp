using System.Collections;
using System.Collections.Generic;


namespace SciSharp.Collections
{
    public class RangeSet : ISet<int>, ICollection<int>
    {
        private readonly int start;
        private int count;
        private bool[] marked;

        public RangeSet(int start, int end)
        {
            this.start = start;
            marked = new bool[end - start + 1];
        }

        public RangeSet(int count)
            : this(0, count - 1) {}

        #region ICollection<int> Members

        public IEnumerator<int> GetEnumerator()
        {
            for (int i = 0; i < marked.Length; i++)
                if (marked[i])
                    yield return i + start;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(int item)
        {
            if (!marked[item])
                count++;

            marked[item + start] = true;
        }

        public void Clear()
        {
            marked = new bool[marked.Length];
            count = 0;
        }

        public void CopyTo(int[] array, int arrayIndex)
        {
            foreach (int i in this)
                array[arrayIndex++] = i;
        }

        public bool Remove(int item)
        {
            if (!marked[item])
                return false;

            count--;
            marked[item] = false;
            return true;
        }

        public int Count
        {
            get { return count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        #endregion

        #region ISet<int> Members

        public bool Contains(int item)
        {
            return marked[item];
        }

        #endregion
    }
}
