namespace SciSharp.Optimization.Aproximate
{
    public class Population
    {
        private readonly int size;
        private double[] fitness;
        private Vector[] vectors;

        public Population(int size)
        {
            this.size = size;
        }

        public Population(params Vector[] population)
        {
            vectors = (Vector[]) population.Clone();
            size = vectors.Length;
        }

        public Population(int size, GenerationFunction generation)
        {
            this.size = size;
            Generate(generation);
        }

        public void Generate(GenerationFunction generation)
        {
            vectors = generation(size);
        }

        public void Evaluate(IRealFunction function)
        {
            fitness = new double[size];

            for (int i = 0; i < vectors.Length; i++)
                fitness[i] = function.Value(vectors[i]);
        }

        public void Breed(BreedingFunction breeding, SelectionFunction selection)
        {
            Vector[] children = breeding(vectors);
            vectors = selection(vectors, children);
        }
    }
}
