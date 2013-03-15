using System.Linq;

using SciSharp.Probabilities;


namespace SciSharp.Benchmarks.Combinatorial
{
    public class TspInstanceWalkFactory : IFactory<TspInstance>
    {
        private readonly int size;
        private readonly int iterations;
        private readonly double multiplier;

        public TspInstanceWalkFactory(int size, int iterations, double multiplier = 1d)
        {
            this.size = size;
            this.iterations = iterations;
            this.multiplier = multiplier;
        }

        public TspInstance Create()
        {
            var costs = Matrices.Ones(size, size);

            for (int i = 0; i < iterations; i++)
            {
                int[] path = Enumerable.Range(0, size).ToArray();
                RandomEx.Instance.Shuffle(path);

                for (int k = 0; k < size; k++)
                {
                    int prev = k > 0 ? k - 1 : size - 1;
                    double d = RandomEx.Instance.Flip() ? 2d : 0.5d;
                    costs[path[prev], path[k]] *= d;
                }
            }

            costs.Normalize();

            costs.Multiply(multiplier);

            return new TspInstance(size, costs.Elements);
        }
    }
}