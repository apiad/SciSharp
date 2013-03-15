namespace SciSharp.Numerics
{
    /// <summary>
    /// Encapsulates a numerical integrator for differential equations than
    /// can be parameterized by a step.
    /// </summary>
    /// <param name="function">The differential function to integrate.</param>
    /// <param name="y">The value of the function (derivatives) at point <paramref name="x"/>.</param>
    /// <param name="x">The current point of integration.</param>
    /// <param name="step">The size of the step.</param>
    public delegate Vector Integrator(DifferentialFunction function, double x, Vector y, double step);
}
