using System;
using System.IO;
using System.Text.RegularExpressions;


namespace SciSharp.Benchmarks.Combinatorial
{
    public class TspInstance : ProblemInstance
    {
        public TspInstance(string file)
            : base(file)
        {
            var regex = new Regex(@"\s+");
            double[] xCoords = null, yCoords = null;

            using (StreamReader reader = File.OpenText(file))
            {
                string line = "";

                // Getting the dimension.
                NumberCities = -1;
                while (NumberCities == -1)
                {
                    line = reader.ReadLine();
                    if (line.StartsWith("DIMENSION"))
                    {
                        NumberCities = int.Parse(line.Substring(11));
                        xCoords = new double[NumberCities];
                        yCoords = new double[NumberCities];
                        Costs = new double[NumberCities, NumberCities];
                    }
                }

                // Getting the coordinates of the cities.
                while (!line.StartsWith("NODE_COORD_SECTION"))
                {
                    line = reader.ReadLine();
                }
                for (int k = 0; k < NumberCities; k++)
                {
                    line = reader.ReadLine();
                    string[] parts = regex.Split(line.Trim());
                    int i = int.Parse(parts[0]) - 1;
                    xCoords[i] = double.Parse(parts[1].Replace('.', ','));
                    yCoords[i] = double.Parse(parts[2].Replace('.', ','));
                }
            }

            // Building the matrix of distances.
            for (int i = 0; i < NumberCities; i++)
            {
                for (int j = 0; j < NumberCities; j++)
                {
                    Costs[i, j] = Math.Sqrt(Math.Pow(xCoords[i] - xCoords[j], 2) +
                                            Math.Pow(yCoords[i] - yCoords[j], 2));
                }
            }
        }

        public TspInstance(int size, double[,] costs)
        {
            NumberCities = size;
            Costs = costs;
        }

        public int NumberCities { get; protected set; }

        public double[,] Costs { get; protected set; }
    }
}
