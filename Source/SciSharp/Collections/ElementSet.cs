using System.Collections.Generic;


namespace SciSharp.Collections
{
    public class ElementSet<T> : FiniteSetBase<T>
    {
        private readonly T element;

        public ElementSet(T element)
        {
            this.element = element;
        }

        public override int Count
        {
            get { return 1; }
        }

        public override bool Contains(T item)
        {
            return item.Equals(element);
        }

        public override IEnumerator<T> GetEnumerator()
        {
            yield return element;
        }
    }
}
