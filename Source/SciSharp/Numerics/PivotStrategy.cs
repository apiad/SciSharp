namespace SciSharp.Numerics
{
    /// <summary>
    /// Encapsulates a pivoting strategy for the selection of a pivot row.
    /// </summary>
    /// <param name="matrix">The matrix that is been transformed.</param>
    /// <param name="index">The index of the column that is been selected in the current iteration.</param>
    /// <param name="dimension">The dimension of the matrix.</param>
    public delegate int PivotStrategy(Matrix matrix, int index, int dimension);
}
