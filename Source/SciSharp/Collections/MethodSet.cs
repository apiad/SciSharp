using System;


namespace SciSharp.Collections
{
    internal class MethodSet<T> : ISet<T>
    {
        private readonly Predicate<T> predicate;

        public MethodSet(Predicate<T> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            this.predicate = predicate;
        }

        #region ISet<T> Members

        public bool Contains(T item)
        {
            return predicate(item);
        }

        #endregion
    }
}
