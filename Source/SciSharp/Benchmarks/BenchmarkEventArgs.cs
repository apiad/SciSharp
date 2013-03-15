using System;


namespace SciSharp.Benchmarks
{
    public class BenchmarkEventArgs : EventArgs
    {
        public BenchmarkEventArgs(BenchmarkFunction function, Vector currentPoint, double currentValue)
        {
            Function = function;
            CurrentPoint = currentPoint;
            CurrentValue = currentValue;
        }

        public BenchmarkFunction Function { get; set; }
        public Vector CurrentPoint { get; set; }
        public double CurrentValue { get; set; }
    }
}
