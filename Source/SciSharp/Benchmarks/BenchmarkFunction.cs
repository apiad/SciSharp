using System;

using SciSharp.Optimization;


namespace SciSharp.Benchmarks
{
    public class BenchmarkFunction : IRealFunction
    {
        private readonly IRealFunction function;
        private readonly Vector minimun;
        private readonly double minimunValue;
        private readonly string name;
        private readonly Vector startingPoint;
        private double error;

        private int evaluations;
        private Vector userMinimun;
        private double userValue;

        public BenchmarkFunction(string name, IRealFunction function, Vector startingPoint)
            : this(name, function, null, startingPoint) {}

        public BenchmarkFunction(string name, IRealFunction function, Vector minimun, Vector startingPoint)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            if (function == null)
                throw new ArgumentNullException("function");

            this.name = name;
            this.function = function;
            this.minimun = minimun;
            this.startingPoint = startingPoint;

            minimunValue = minimun != null ? function.Value(minimun) : double.NaN;

            Reset();
        }

        public Vector StartingPoint
        {
            get { return startingPoint; }
        }

        public double MinimunValue
        {
            get { return minimunValue; }
        }

        public Vector Minimun
        {
            get { return minimun; }
        }

        public IRealFunction Function
        {
            get { return function; }
        }

        #region IRealFunction Members

        public virtual int Dimension
        {
            get { return function.Dimension; }
        }

        public virtual double Value(Vector x)
        {
            evaluations++;
            double value = function.Value(x);

            Debug.ThrowOnNaN(value, "A NaN value was found at {0}".Formatted(x));

            OnEvaluation(new BenchmarkEventArgs(this, x, value));

            if (value < userValue)
            {
                error = double.IsNaN(minimunValue) ? double.NaN : value - minimunValue;

                userValue = value;
                userMinimun = x;
            }

            return value;
        }

        #endregion

        public event EventHandler<BenchmarkEventArgs> Evaluation;

        protected virtual void OnEvaluation(BenchmarkEventArgs e)
        {
            EventHandler<BenchmarkEventArgs> handler = Evaluation;
            if (handler != null)
                handler(this, e);
        }

        internal BenchmarkResult Result(OptimizerResult optimizerResult)
        {
            return new BenchmarkResult(name, minimunValue, minimun, evaluations, userMinimun, userValue, error,
                                       function.Dimension, optimizerResult);
        }

        internal void Reset()
        {
            error = int.MaxValue;
            evaluations = 0;
            userMinimun = null;
            userValue = int.MaxValue;
        }
    }
}
