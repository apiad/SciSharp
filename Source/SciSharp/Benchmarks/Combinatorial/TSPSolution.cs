using System.IO;
using System.Linq;

using SciSharp.Probabilities;


namespace SciSharp.Benchmarks.Combinatorial
{
    public class TspSolution : ProblemSolution
    {
        public TspSolution(TspInstance instance)
            : base(instance)
        {
            Instance = instance;
            Path = new int[Instance.NumberCities];
        }

        public TspInstance Instance { get; protected set; }

        public int[] Path { get; protected set; }

        public override void Load(string file)
        {
            using (StreamReader reader = File.OpenText(file))
            {
                string line = "";

                // Skip the fitness calculated by the algorithm.
                line = reader.ReadLine();
                while (line.Trim() == "")
                {
                    line = reader.ReadLine();
                }

                // Skip the number of cities;
                line = reader.ReadLine();
                while (line.Trim() == "")
                {
                    line = reader.ReadLine();
                }

                // Get the path.
                for (int i = 0; i < Instance.NumberCities; i++)
                {
                    line = reader.ReadLine();
                    while (line.Trim() == "")
                    {
                        line = reader.ReadLine();
                    }
                    Path[i] = int.Parse(line) - 1;
                }
            }
        }

        public override double Fitness()
        {
            double fitness = 0;

            for (int i = 1; i < Path.Length; i++)
            {
                fitness += Instance.Costs[Path[i - 1], Path[i]];
            }
            fitness += Instance.Costs[Path[Path.Length - 1], Path[0]];

            return fitness;
        }

        public override bool IsFeasible()
        {
            var present = new bool[Instance.NumberCities];

            for (int i = 0; i < Instance.NumberCities; i++)
            {
                if (Path[i] < 0 || Path[i] >= Instance.NumberCities)
                {
                    return false;
                }
                present[Path[i]] = true;
            }

            for (int i = 0; i < Instance.NumberCities; i++)
            {
                if (!present[i])
                {
                    return false;
                }
            }

            return true;
        }

        public static TspSolution Random(TspInstance instance, RandomEx random  )
        {
            int[] path = Enumerable.Range(0, instance.NumberCities).ToArray();
            random.Shuffle(path);

            return new TspSolution(instance) { Path = path };
        }
    }
}
