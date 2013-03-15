using System.IO;
using System.Text.RegularExpressions;


namespace SciSharp.Benchmarks.Combinatorial
{
    internal class TwoSPSolution : ProblemSolution
    {
        public TwoSPSolution(TwoSPInstance instance)
            : base(instance)
        {
            Instance = instance;
            Coordinates = new int[Instance.NumberItems,Instance.NumberItems];
        }

        public TwoSPInstance Instance { get; protected set; }

        public int[,] Coordinates { get; protected set; }

        public override void Load(string file)
        {
            var regex = new Regex(@"\s+");

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

                // Getting the coordinates of the strips.
                for (int i = 0; i < Instance.NumberItems; i++)
                {
                    line = reader.ReadLine();
                    while (line.Trim() == "")
                    {
                        line = reader.ReadLine();
                    }
                    string[] parts = regex.Split(line.Trim());
                    Coordinates[i, 0] = int.Parse(parts[0]);
                    Coordinates[i, 1] = int.Parse(parts[1]);
                }
            }
        }

        public override double Fitness()
        {
            double fitness = 0;

            for (int i = 0; i < Coordinates.GetLength(0); i++)
            {
                int height = Coordinates[i, 1] + Instance.ItemsHeight[i];
                if (height > fitness)
                {
                    fitness = height;
                }
            }

            return fitness;
        }

        public override bool IsFeasible()
        {
            for (int item = 0; item < Coordinates.GetLength(0); item++)
            {
                int x = 0, y = 1;
                int itemXStart = Coordinates[item, x];
                int itemXEnd = Coordinates[item, x] + Instance.ItemsWidth[item];
                int itemYStart = Coordinates[item, y];
                int itemYEnd = Coordinates[item, y] + Instance.ItemsHeight[item];

                // Checking if the item is located inside the strip.
                if (itemXStart < 0 || itemXEnd > Instance.StripWidth)
                {
                    return false;
                }

                // Check if the item collapses with other item.
                for (int otherItem = 0; otherItem < Coordinates.GetLength(0); otherItem++)
                {
                    if (otherItem != item)
                    {
                        int otherItemXStart = Coordinates[otherItem, x];
                        int otherItemXEnd = Coordinates[otherItem, x] + Instance.ItemsWidth[otherItem];
                        int otherItemYStart = Coordinates[otherItem, y];
                        int otherItemYEnd = Coordinates[otherItem, y] + Instance.ItemsHeight[otherItem];

                        if (((otherItemXStart >= itemXStart && otherItemXStart < itemXEnd) ||
                             (otherItemXEnd > itemXStart && otherItemXEnd <= itemXEnd)) &&
                            ((otherItemYStart >= itemYStart && otherItemYStart < itemYEnd) ||
                             (otherItemYEnd > itemYStart && otherItemYEnd <= itemYEnd)))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }
    }
}
