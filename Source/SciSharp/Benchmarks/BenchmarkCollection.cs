using System;
using System.Collections;
using System.Collections.Generic;

using SciSharp.Optimization;


namespace SciSharp.Benchmarks
{
    public class BenchmarkCollection : IEnumerable<BenchmarkFunction>
    {
        private readonly List<BenchmarkFunction> data;

        public BenchmarkCollection()
        {
            data = new List<BenchmarkFunction>();
        }

        #region IEnumerable<BenchmarkFunction> Members

        IEnumerator<BenchmarkFunction> IEnumerable<BenchmarkFunction>.GetEnumerator()
        {
            return data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return data.GetEnumerator();
        }

        #endregion

        public void Add(string name, IRealFunction function, Vector minimun, Vector startingPoint)
        {
            data.Add(new BenchmarkFunction(name, function, minimun, startingPoint));
        }

        public void Add(BenchmarkFunction benchmarkData)
        {
            benchmarkData.Reset();
            data.Add(benchmarkData);
        }

        public Benchmark Apply(IOptimizer optimizer)
        {
            return new Benchmark(optimizer, RunTest(optimizer));
        }

        public event EventHandler<BenchmarkEventArgs> Evaluation
        {
            add
            {
                foreach (BenchmarkFunction function in data)
                    function.Evaluation += value;
            }
            remove
            {
                foreach (BenchmarkFunction function in data)
                    function.Evaluation -= value;
            }
        }

        private IEnumerable<BenchmarkResult> RunTest(IOptimizer optimizer)
        {
            foreach (BenchmarkFunction d in data)
            {
                // Run the optimizer
                OptimizerResult result = optimizer.Run(d, d.StartingPoint);
                BenchmarkResult benchmarkResult = d.Result(result);

                // Next step
                yield return benchmarkResult;

                // Reset the benchmarking function
                d.Reset();
            }
        }
    }
}
