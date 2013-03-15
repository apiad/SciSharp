using System;
using System.Collections;
using System.Collections.Generic;


namespace SciSharp.Collections
{
    public class RandomSet : IEnumerable<int>
    {
        private readonly int count;
        private readonly int hi;
        private readonly int low;
        private readonly Random rand;

        public RandomSet(int count)
            : this(int.MaxValue, count) {}

        public RandomSet(int hi, int count)
            : this(0, hi, count) {}

        public RandomSet(int low, int hi, int count)
        {
            this.low = low;
            this.hi = hi;
            this.count = count;
            rand = new Random();
        }

        #region IEnumerable<int> Members

        public IEnumerator<int> GetEnumerator()
        {
            for (int i = 0; i < count; i++)
                yield return rand.Next(low, hi);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
