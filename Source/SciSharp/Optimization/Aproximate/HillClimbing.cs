namespace SciSharp.Optimization.Aproximate
{
    public class HillClimbing : IOptimizer
    {
        private readonly int iterations;
        private readonly NeighborhoodFunction neighborhoodFunction;
        private readonly LocalSelector selector;

        public HillClimbing(NeighborhoodFunction neighborhoodFunction, int iterations, LocalSelector selector)
        {
            this.neighborhoodFunction = neighborhoodFunction;
            this.iterations = iterations;
            this.selector = selector;
        }

        public LocalSelector Selector
        {
            get { return selector; }
        }

        public NeighborhoodFunction NeighborhoodFunction
        {
            get { return neighborhoodFunction; }
        }

        public int Iterations
        {
            get { return iterations; }
        }

        #region IOptimizer Members

        public OptimizerResult Run(IRealFunction function, Vector startingPoint)
        {
            Vector current = startingPoint;

            for (int i = 0; i < Iterations; i++)
                current = Selector(current, function, NeighborhoodFunction(current));

            return new OptimizerResult(current, function.Value(current));
        }

        #endregion
    }
}
