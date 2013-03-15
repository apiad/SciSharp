namespace SciSharp.Collections
{
    public interface IQueue<T>
    {
        int Count { get; }

        void Enqueue(T item);

        T Peek();

        T Dequeue();
    }
}
