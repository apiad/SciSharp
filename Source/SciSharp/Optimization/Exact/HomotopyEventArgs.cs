using System;


namespace SciSharp.Optimization.Exact
{
    public class HomotopyEventArgs : EventArgs
    {
        public HomotopyEventArgs(int iteration, IRealFunction function, Vector point, Vector gradient, double parameter)
        {
            Iteration = iteration;
            Function = function;
            Point = point;
            Gradient = gradient;
            Parameter = parameter;
        }

        public int Iteration { get; set; }
        public IRealFunction Function { get; set; }
        public Vector Point { get; set; }
        public Vector Gradient { get; set; }
        public double Parameter { get; set; }
    }
}
