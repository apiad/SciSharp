using System;
using System.Collections.Generic;


namespace SciSharp.Collections
{
    public class ArraySet<T> : FiniteSetBase<T>
    {
        private readonly T[] items;

        public ArraySet(params T[] items)
        {
            this.items = items;
            Array.Sort(this.items);
        }

        public override int Count
        {
            get { return items.Length; }
        }

        public override bool Contains(T item)
        {
            return Array.BinarySearch(items, item) >= 0;
        }

        public override IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>) items).GetEnumerator();
        }

        public override bool Equals(object obj)
        {
            var array = obj as ArraySet<T>;

            if (array != null)
                return Equals(array);

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            if (items.Length == 0)
                return 0;

            int code = items[0].GetHashCode();

            for (int i = 1; i < Count; i++)
                code = (code ^ 397)*items[i].GetHashCode();

            return code;
        }

        public bool Equals(ArraySet<T> other)
        {
            if (other == null)
                throw new ArgumentNullException("other");

            if (other.Count != Count)
                return false;

            for (int i = 0; i < Count; i++)
                if (items[i].Equals(other.items[i]))
                    return false;

            return true;
        }
    }
}
