using System;

using SciSharp.DirectCompute;


namespace SciSharp.Performance.GpuMatrixBenchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            DxContext context = null;

            int creationTime = Engine.Meassure(() => context = new DxContext());

            Console.WriteLine("Context creation: {0} ms.", creationTime);
            Console.WriteLine();

            #region Adding

            for (int i = 1; i <= 4; i++)
            {
                int size = i * 1000;

                Console.WriteLine("Adding matrices ({0} x {0})", size);

                var m1 = new Matrix(size, size);
                var m2 = new Matrix(size, size);

                int timeCpu = Engine.Meassure(() => Matrix.Add(m1, m2));

                Console.WriteLine("CPU time: {0} ms.", timeCpu);

                var g1 = new DxMatrix(context, size, size);
                var g2 = new DxMatrix(context, size, size);

                int timeGpu = Engine.Meassure(() => DxMatrix.Add(g1, g2));

                double multiplier = timeCpu * 1d / timeGpu;
                Console.WriteLine("GPU time: {0} ms. ({1:0.0}x faster)", timeGpu, multiplier);

                Console.WriteLine();

                g1.Dispose();
                g2.Dispose();
            }

            #endregion

            #region Multiplying

            for (int i = 1; i <= 5; i++)
            {
                int size = i * 100;

                Console.WriteLine("Multiplying matrices ({0} x {0})", size);

                var m1 = new Matrix(size, size);
                var m2 = new Matrix(size, size);

                int timeCpu = Engine.Meassure(() => Matrix.Multiply(m1, m2));

                Console.WriteLine("CPU time: {0} ms.", timeCpu);

                var g1 = new DxMatrix(context, size, size);
                var g2 = new DxMatrix(context, size, size);

                int timeGpu = Engine.Meassure(() => DxMatrix.Multiply(g1, g2));

                double multiplier = timeCpu * 1d / timeGpu;
                Console.WriteLine("GPU time: {0} ms. ({1:0.0}x faster)", timeGpu, multiplier);

                Console.WriteLine();

                g1.Dispose();
                g2.Dispose();
            }

            for (int i = 1; i <= 8; i++)
            {
                int size = i * 1000;

                Console.WriteLine("Multiplying bigger matrices ({0} x {0})", size);

                var g1 = new DxMatrix(context, size, size);
                var g2 = new DxMatrix(context, size, size);

                int timeGpu = Engine.Meassure(() => DxMatrix.Multiply(g1, g2));

                Console.WriteLine("GPU time: {0} ms.", timeGpu);
                Console.WriteLine();

                g1.Dispose();
                g2.Dispose();
            }

            #endregion
        }
    }
}
