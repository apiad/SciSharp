namespace SciSharp.Numerics
{
    public static class MultiStepCorrectors
    {
        public static Vector AdamsMoulton(DifferentialFunction function, double x, Vector[] y, Vector yIter,
                                          double step)
        {
            return y[0] + step/24*
                   (9*function(x + step, yIter) + 19*function(x, y[0]) -
                    5*function(x - step, y[1]) + function(x - 2*step, y[2]));
        }
    }
}
