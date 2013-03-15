namespace SciSharp.Collections
{
    public interface IFiniteSet<T> : IEnumerableSet<T>
    {
        int Count { get; }

        T[] ToArray();
    }
}
