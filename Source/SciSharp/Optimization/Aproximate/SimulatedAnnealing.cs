using System;


namespace SciSharp.Optimization.Aproximate
{
    public class SimulatedAnnealing : IOptimizer
    {
        private readonly NeighborhoodFunction neighborhoodFunction;
        private readonly TemperatureFunction temperatureFunction;

        public SimulatedAnnealing(TemperatureFunction temperatureFunction,
                                  NeighborhoodFunction neighborhoodFunction)
        {
            if (temperatureFunction == null)
                throw new ArgumentNullException("temperatureFunction");

            if (neighborhoodFunction == null)
                throw new ArgumentNullException("neighborhoodFunction");

            this.temperatureFunction = temperatureFunction;
            this.neighborhoodFunction = neighborhoodFunction;
        }

        public TemperatureFunction TemperatureFunction
        {
            get { return temperatureFunction; }
        }

        public NeighborhoodFunction NeighborhoodFunction
        {
            get { return neighborhoodFunction; }
        }

        #region IOptimizer Members

        public OptimizerResult Run(IRealFunction function, Vector startingPoint)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
