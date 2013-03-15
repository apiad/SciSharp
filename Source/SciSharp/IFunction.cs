namespace SciSharp
{
    public interface IFunction<in T, out TResult>
    {
        TResult Value(T x);
    }
}
