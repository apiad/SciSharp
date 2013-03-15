using System;
using System.Collections.Generic;


namespace SciSharp
{
    public class MethodComparer<T> : IComparer<T>
    {
        private readonly Comparison<T> cmp;

        public MethodComparer(Comparison<T> cmp)
        {
            if (cmp == null)
                throw new ArgumentNullException("cmp");

            this.cmp = cmp;
        }

        #region IComparer<T> Members

        public int Compare(T x, T y)
        {
            return cmp(x, y);
        }

        #endregion
    }
}
