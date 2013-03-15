using System;
using System.Threading;

using SciSharp.Probabilities;


namespace SciSharp.Examples.NServersParallelSimulation
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            int servers = Convert.ToInt32(Input("Servers: "));
            double client = 1d/Convert.ToDouble(Input("Clients arrival time: "));
            double server = 1d/Convert.ToDouble(Input("Servers processing time: "));
            double close = Convert.ToDouble(Input("Closing time: "));

            var simulator = new NServersParallelSimulator(servers, RandomVariable.Exponential(client),
                                                          RandomVariable.Exponential(server), close);

            Console.WriteLine("Press enter to begin simulation...");
            Console.ReadLine();

            while (simulator.NextEvent())
            {
                Draw(simulator);

                if (simulator.NextTime > simulator.CurrentTime)
                    Thread.Sleep((int) ((simulator.NextTime - simulator.CurrentTime)*200));
            }
        }

        private static void Draw(NServersParallelSimulator simulator)
        {
            Console.Clear();
            Console.WriteLine("Time: {0}\n", simulator.CurrentTime);
            Console.WriteLine("Servers:");

            for (int i = 0; i < simulator.Servers; i++)
            {
                int c = simulator.ServerBusy(i);
                Console.WriteLine(c > 0 ? "{0}" : "-", c);
            }

            Console.WriteLine();

            Console.Write("Queue: ");
            for (int i = 0; i < simulator.ClientsInQueue; i++)
                Console.Write("{0}, ", simulator.Clients + i);

            Console.WriteLine();
        }

        private static string Input(string prompt)
        {
            string s = "";

            while (string.IsNullOrEmpty(s))
            {
                Console.Write(prompt);
                s = Console.ReadLine();
            }

            return s;
        }
    }
}
