using System.Collections.Generic;


namespace SciSharp.Collections
{
    public interface ITree<out T>
    {
        T Value { get; }
        IEnumerable<ITree<T>> Children { get; }
    }
}
