using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using SciSharp.Optimization;


namespace SciSharp.Benchmarks
{
    public class Benchmark : IEnumerable<BenchmarkResult>
    {
        private readonly IOptimizer optimizer;
        private readonly IEnumerable<BenchmarkResult> results;

        public Benchmark(IOptimizer optimizer, IEnumerable<BenchmarkResult> results)
        {
            this.optimizer = optimizer;
            this.results = results;
        }

        #region IEnumerable<BenchmarkResult> Members

        public IEnumerator<BenchmarkResult> GetEnumerator()
        {
            return results.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        public override string ToString()
        {
            return "Benchmarking {0}".Formatted(optimizer);
        }

        public void Run()
        {
            Run(TextWriter.Null);
        }

        public void Run(TextWriter log)
        {
            if (log == null)
                throw new ArgumentNullException("log");

            log.WriteLine(this);
            log.WriteLine();

            foreach (BenchmarkResult result in results)
            {
                log.WriteLine(result);
                log.WriteLine();
            }
        }
    }
}
