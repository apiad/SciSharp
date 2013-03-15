using System.IO;


namespace SciSharp.Benchmarks.Combinatorial
{
    internal class QAPSolution : ProblemSolution
    {
        public QAPSolution(QAPInstance instance)
            : base(instance)
        {
            Instance = instance;
            Assignment = new int[Instance.NumberFacilities];
        }

        public QAPInstance Instance { get; protected set; }

        public int[] Assignment { get; protected set; }

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

                // Skip the number of locations and facilities;
                line = reader.ReadLine();
                while (line.Trim() == "")
                {
                    line = reader.ReadLine();
                }

                // Get the assignment of the facilities to the locations.
                for (int i = 0; i < Instance.NumberFacilities; i++)
                {
                    line = reader.ReadLine();
                    while (line.Trim() == "")
                    {
                        line = reader.ReadLine();
                    }
                    Assignment[i] = int.Parse(line) - 1;
                }
            }
        }

        public override double Fitness()
        {
            double fitness = 0;

            for (int i = 0; i < Instance.NumberFacilities; i++)
            {
                for (int j = 0; j < Instance.NumberFacilities; j++)
                {
                    fitness += Instance.Distances[i, j]*Instance.Flows[Assignment[i], Assignment[j]];
                }
            }

            return fitness;
        }

        public override bool IsFeasible()
        {
            var present = new bool[Instance.NumberFacilities];

            for (int i = 0; i < Instance.NumberFacilities; i++)
            {
                if (Assignment[i] < 0 || Assignment[i] >= Instance.NumberFacilities)
                {
                    return false;
                }
                present[Assignment[i]] = true;
            }

            for (int i = 0; i < Instance.NumberFacilities; i++)
            {
                if (!present[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
