namespace SciSharp.Numerics
{
    public static class SingleStepMethods
    {
        public static Vector Euler(DifferentialFunction function, double x, Vector y, double step)
        {
            return y + step*function(x, y);
        }

        public static Vector RungeKutta2(DifferentialFunction function, double x, Vector y, double step)
        {
            Vector k1 = step*function(x, y);
            Vector k2 = step*function(x + step, y + k1);

            return y + 1d/2*(k1 + k2);
        }

        public static Vector RungeKutta4(DifferentialFunction function, double x, Vector y, double step)
        {
            Vector k1 = step*function(x, y);
            Vector k2 = step*function(x + step/2, y + k1/2);
            Vector k3 = step*function(x + step/2, y + k2/2);
            Vector k4 = step*function(x + step, y + k3);

            return y + 1d/6*(k1 + 2*k2 + 2*k3 + k4);
        }
    }
}
