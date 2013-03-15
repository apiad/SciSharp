using System;
using System.Collections;
using System.Collections.Generic;


namespace SciSharp.Collections
{
    public abstract class FiniteSetBase<T> : IFiniteSet<T>
    {
        #region IFiniteSet<T> Members

        public abstract bool Contains(T item);

        public abstract IEnumerator<T> GetEnumerator();

        public abstract int Count { get; }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public T[] ToArray()
        {
            throw new NotImplementedException();
        }

        #endregion

        public override string ToString()
        {
            return this.ToString<T>();
        }

        public override bool Equals(object obj)
        {
            return obj is IFiniteSet<T> && this.Equals<T>(obj as IFiniteSet<T>);
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }
    }
}
