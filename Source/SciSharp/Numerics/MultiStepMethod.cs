namespace SciSharp.Numerics
{
    public delegate Vector MultiStepMethod(DifferentialFunction function, double x, Vector[] y, double step);
}
