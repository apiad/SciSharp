using SciSharp.Probabilities;


namespace SciSharp.Benchmarks.Combinatorial
{
    public class TspInstanceDistributionFactory : IFactory<TspInstance>
    {
        private readonly int size;
        private readonly RandomVariable costDistribution;

        public TspInstanceDistributionFactory(int size, RandomVariable costDistribution)
        {
            this.size = size;
            this.costDistribution = costDistribution;
        }

        public TspInstance Create()
        {
            var instance = new TspInstance(size, new double[size,size]);

            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    instance.Costs[i, j] = costDistribution;

            return instance;
        }
    }
}