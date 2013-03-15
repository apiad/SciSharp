namespace SciSharp.Numerics
{
    /// <summary>
    /// Encapsulates a linear solving algorithm for the problem Ax = b.
    /// </summary>
    /// <param name="matrix">The coefficient matrix (A).</param>
    /// <param name="vector">The independent terms vector (b).</param>
    public delegate LinearSolution LinearSolver(Matrix matrix, Vector vector);
}
