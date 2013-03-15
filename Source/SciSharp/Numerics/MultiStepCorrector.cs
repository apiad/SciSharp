namespace SciSharp.Numerics
{
    public delegate Vector MultiStepCorrector(
        DifferentialFunction function, double x, Vector[] y, Vector yIter, double step);
}
