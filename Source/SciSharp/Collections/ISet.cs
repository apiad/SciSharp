namespace SciSharp.Collections
{
    public interface ISet<in T>
    {
        bool Contains(T item);
    }
}
