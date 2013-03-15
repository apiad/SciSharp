namespace SciSharp.Numerics
{
    public static class SingleStepCorrectors
    {
        public static Vector Method(DifferentialFunction function, double x, Vector y, Vector yIter,
                                    double step)
        {
            return y + step/2*(function(x, y) + function(x + step, yIter));
        }
    }
}
