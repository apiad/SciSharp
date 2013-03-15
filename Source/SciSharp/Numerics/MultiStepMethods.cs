namespace SciSharp.Numerics
{
    public static class MultiStepMethods
    {
        public static Vector AdamBashforth(DifferentialFunction function, double x, Vector[] y, double step)
        {
            return y[0] + step/24*
                   (55*function(x, y[0]) - 59*function(x - step, y[1]) + 37*function(x - 2*step, y[2]) -
                    9*function(x - 3*step, y[3]));
        }

        public static Vector Method1(DifferentialFunction function, double x, Vector[] y, double step)
        {
            return y[1] + 2*step*function(x, y[0]);
        }

        public static Vector Milne(DifferentialFunction function, double x, Vector[] y, double step)
        {
            return y[3] + 4*step/3*(2*function(x, y[0]) -
                                    function(x - step, y[1]) + 2*function(x - 2*step, y[2]));
        }
    }
}
