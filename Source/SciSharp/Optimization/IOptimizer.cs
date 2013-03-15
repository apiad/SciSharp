namespace SciSharp.Optimization
{
    public interface IOptimizer<in TFunction, in T, out TResult>
        where TFunction : IFunction<T, TResult>
    {
        OptimizerResult Run(TFunction function, Vector startingPoint);
    }

    public interface IOptimizer : IOptimizer<IRealFunction, Vector, double> {}
}
