using System;
using System.IO;


namespace SciSharp.Benchmarks.Combinatorial
{
    internal class SPPSolution : ProblemSolution
    {
        public SPPSolution(SPPInstance instance)
            : base(instance)
        {
            Instance = instance;
            Assignment = new int[Instance.NumberItems];
        }

        public SPPInstance Instance { get; protected set; }

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

                // Skip the number of items.
                line = reader.ReadLine();
                while (line.Trim() == "")
                {
                    line = reader.ReadLine();
                }

                // Skip the number of subsets.
                line = reader.ReadLine();
                while (line.Trim() == "")
                {
                    line = reader.ReadLine();
                }

                // Get the assignment of subsets.
                for (int i = 0; i < Instance.NumberItems; i++)
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
            double deviation = 0;

            for (int subset = 0; subset < Instance.NumberSubsets; subset++)
            {
                double subsetWeight = 0;
                for (int item = 0; item < Instance.NumberItems; item++)
                {
                    if (subset == Assignment[item])
                    {
                        subsetWeight += Instance.ItemsWeight[item];
                    }
                }
                deviation += Math.Abs(subsetWeight - Instance.SubsetsWeight[subset]);
            }

            return deviation;
        }

        public override bool IsFeasible()
        {
            for (int i = 0; i < Instance.NumberItems; i++)
            {
                if (Assignment[i] < 0 || Assignment[i] >= Instance.NumberSubsets)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
