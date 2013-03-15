namespace SciSharp.Optimization
{
    public class OptimizerResult
    {
        public static readonly OptimizerResult Fail = new OptimizerResult(OptimizerStatus.GeneralFailure);
        public static readonly OptimizerResult MaxIterations = new OptimizerResult(OptimizerStatus.MaxIterationsReached);

        public static readonly OptimizerResult NonConvergence =
            new OptimizerResult(OptimizerStatus.NonConvergenceDetected);

        public static readonly OptimizerResult InternalError =
            new OptimizerResult(OptimizerStatus.InternalInvariantError);

        private readonly Vector minimun;
        private readonly OptimizerStatus status;
        private readonly bool success;
        private readonly double value;

        public OptimizerResult(Vector minimun, double value)
        {
            success = true;
            status = OptimizerStatus.Success;
            this.minimun = minimun;
            this.value = value;
        }

        private OptimizerResult(OptimizerStatus status)
        {
            success = false;
            this.status = status;
        }

        public bool Success
        {
            get { return success; }
        }

        public Vector Minimun
        {
            get { return minimun; }
        }

        public double Value
        {
            get { return value; }
        }

        public override string ToString()
        {
            return success ? "{0} at {1}".Formatted(value, minimun) : "(!) {0}".Formatted(status);
        }
    }
}
