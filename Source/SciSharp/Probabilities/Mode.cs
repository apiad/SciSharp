using System;
using System.Collections;
using System.Collections.Generic;

using SciSharp.Collections;


namespace SciSharp.Probabilities
{
    public class Mode<T> : IEnumerable<T>
    {
        private readonly DefaultDictionary<T, int> count;
        private T mode;
        private int modeCount;
        private int version;

        public Mode()
        {
            count = new DefaultDictionary<T, int>();
        }

        public T Get
        {
            get
            {
                if (modeCount == 0)
                    throw new InvalidOperationException("There is no data.");

                return mode;
            }
        }

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            int callVersion = version;

            foreach (var item in count)
                for (int i = 0; i < item.Value; i++)
                {
                    if (version != callVersion)
                        throw new InvalidOperationException("The collection was modified while iterated.");

                    yield return item.Key;
                }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        public void Add(T item)
        {
            int c = ++count[item];

            if (c > modeCount)
            {
                modeCount = c;
                mode = item;
            }

            version++;
        }
    }
}
