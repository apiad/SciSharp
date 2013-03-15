namespace SciSharp.Numerics
{
    public delegate Vector SingleStepCorrector(
        DifferentialFunction function, double x, Vector y, Vector yIter, double step);
}
