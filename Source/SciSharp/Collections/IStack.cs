namespace SciSharp.Collections
{
    public interface IStack<T>
    {
        #region Properties

        T this[int index] { get; }

        int Count { get; }

        #endregion

        #region Members

        void Push(T item);

        T Top();

        T Pop();

        void Clear();

        #endregion
    }
}
