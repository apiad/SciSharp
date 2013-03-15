namespace SciSharp.Numerics
{

    #region Differential Equations

    /// <summary>
    /// Encapsulates a single step integration method.
    /// </summary>
    /// <param name="function">The differential function to be integrated.</param>
    /// <param name="x">The current point of integration.</param>
    /// <param name="y">The value of the function at the point <paramref name="x"/>.</param>
    /// <param name="step">The integration step, can be positive or negative.</param>
    public delegate Vector SingleStepMethod(DifferentialFunction function, double x, Vector y, double step);

    #endregion

    #region Linear Systems

    #endregion
}
