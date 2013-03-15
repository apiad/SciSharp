using System.IO;
using System.Text.RegularExpressions;


namespace SciSharp.Benchmarks.Combinatorial
{
    internal class QAPInstance : ProblemInstance
    {
        public QAPInstance(string file)
            : base(file)
        {
            var regex = new Regex(@"\s+");

            using (StreamReader reader = File.OpenText(file))
            {
                string line = "";

                // Getting the dimension.
                line = reader.ReadLine();
                while (line.Trim() == "")
                {
                    line = reader.ReadLine();
                }
                NumberFacilities = int.Parse(line.Trim());

                // Getting the flow matrix.
                Flows = new double[NumberFacilities, NumberFacilities];
                for (int i = 0; i < NumberFacilities; i++)
                {
                    line = reader.ReadLine();
                    while (line.Trim() == "")
                    {
                        line = reader.ReadLine();
                    }
                    string[] parts = regex.Split(line.Trim());
                    for (int j = 0; j < NumberFacilities; j++)
                    {
                        Flows[i, j] = double.Parse(parts[j]);
                    }
                }

                // Getting the distance matrix.			
                Distances = new double[NumberFacilities, NumberFacilities];
                for (int i = 0; i < NumberFacilities; i++)
                {
                    line = reader.ReadLine();
                    while (line.Trim() == "")
                    {
                        line = reader.ReadLine();
                    }
                    string[] parts = regex.Split(line.Trim());
                    for (int j = 0; j < NumberFacilities; j++)
                    {
                        Distances[i, j] = double.Parse(parts[j]);
                    }
                }
            }
        }

        public int NumberFacilities { get; protected set; }

        public double[,] Distances { get; protected set; }

        public double[,] Flows { get; protected set; }
    }
}
