using SciSharp.Optimization;


namespace SciSharp.Benchmarks
{
    public struct BenchmarkResult
    {
        private readonly int dimension;
        private readonly double error;
        private readonly int evaluations;
        private readonly Vector minimun;
        private readonly double minimunValue;
        private readonly string name;
        private readonly OptimizerResult optimizerResult;
        private readonly Vector userMinimun;
        private readonly double userValue;

        public BenchmarkResult(string name, double minimunValue, Vector minimun, int evaluations,
                               Vector userMinimun, double userValue, double error,
                               int dimension, OptimizerResult optimizerResult)
        {
            this.name = name;
            this.minimunValue = minimunValue;
            this.minimun = minimun;
            this.evaluations = evaluations;
            this.userMinimun = userMinimun;
            this.userValue = userValue;
            this.error = error;
            this.dimension = dimension;
            this.optimizerResult = optimizerResult;
        }

        public double MinimunValue
        {
            get { return minimunValue; }
        }

        public Vector Minimun
        {
            get { return minimun; }
        }

        public int Evaluations
        {
            get { return evaluations; }
        }

        public Vector UserMinimun
        {
            get { return userMinimun; }
        }

        public double UserValue
        {
            get { return userValue; }
        }

        public double Error
        {
            get { return error; }
        }

        public string Name
        {
            get { return name; }
        }

        public override string ToString()
        {
            return string.Format("{0} Function (D-{6}) [{2} evaluations]\nGlobal minimun: {1}\nUser minimun: {3} at {4}\nError: {5}\nOptimizer ouput: {7}\n",
                                 name, double.IsNaN(minimunValue) ? "Not Available" : minimunValue.ToString() + " at " + minimun,
                                 evaluations, userValue, userMinimun, double.IsNaN(error) ? "Not Available" : error.ToString(),
                                 dimension, optimizerResult);
        }
    }
}
