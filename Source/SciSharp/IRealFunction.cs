namespace SciSharp
{
    public interface IRealFunction : IFunction<Vector, double>
    {
        int Dimension { get; }
    }
}
