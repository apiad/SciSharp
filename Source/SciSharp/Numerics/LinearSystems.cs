using System;
using System.Threading.Tasks;


namespace SciSharp.Numerics
{
    /// <summary>
    /// Contains elementary algebraic methods for solving linear systems.
    /// </summary>
    public static class LinearSystems
    {
        #region Factorizations

        /// <summary>
        /// Attempts to perform a PLU factorization on a given matrix. If the 
        /// matrix is invertible, after the method returns the <paramref name="matrix"/>
        /// instance is PLU-factorized.
        /// </summary>
        /// <param name="matrix">The matrix to factorize.</param>
        /// <param name="pivotStrategy">A pivoting strategy for selecting the pivot row.</param>
        /// <param name="singularColumn">If the factorization fails (i.e. the matrix is singular)
        /// this parameter indicates the first column which could not be factorized.</param>
        /// <returns>True if the matrix could be factorized, false otherwise.</returns>
        /// <exception cref="ArgumentNullException">If any of the arguments is null.</exception>
        public static bool TryPluFactorization(Matrix matrix, PivotStrategy pivotStrategy, out int singularColumn)
        {
            Debug.Post("LinearSystems.TryPluFactorization.");

            if (matrix == null)
                throw new ArgumentNullException("matrix");

            if (pivotStrategy == null)
                throw new ArgumentNullException("pivotStrategy");

            Debug.Indent();
            Debug.Post("Matrix size: {0}.", matrix.Rows);
            Debug.BeginTimer();

            int dim = Math.Min(matrix.Rows, matrix.Columns);

            for (int k = 0; k < dim - 1; k++)
            {
                int pivot = pivotStrategy(matrix, k, dim);

                matrix.SwapRows(pivot, k);

                if (Math.Abs(matrix[k, k]) <= Engine.Epsilon)
                {
                    singularColumn = k;

                    Debug.EndTimer();
                    Debug.Unindent();

                    return false;
                }

                if (!Engine.UseParallelOptimizations)
                {
                    // Secuencial

                    for (int i = k + 1; i < dim; i++)
                    {
                        double m = matrix[i, k] = matrix[i, k]/matrix[k, k];

                        for (int j = k + 1; j < dim; j++)
                            matrix[i, j] -= m*matrix[k, j];
                    }
                }
                else
                {
                    // Paralelo

                    int p = k;

                    Parallel.For(k + 1, dim, i =>
                                             {
                                                 double m = matrix[i, p] = matrix[i, p]/matrix[p, p];

                                                 for (int j = p + 1; j < dim; j++)
                                                     matrix[i, j] -= m*matrix[p, j];
                                             });
                }
            }

            if (Math.Abs(matrix[dim - 1, dim - 1]) <= Engine.Epsilon)
            {
                singularColumn = dim - 1;

                Debug.EndTimer();
                Debug.Unindent();

                return false;
            }

            Debug.EndTimer();
            Debug.Unindent();

            singularColumn = -1;

            return true;
        }

        public static Factorization PluFactorization(Matrix matrix, PivotStrategy pivotStrategy)
        {
            Debug.Post("LinearSystems.GaussElimination.");
            Debug.BeginTimer();
            Debug.Indent();

            int singularColumn;

            if (!TryPluFactorization(matrix, pivotStrategy, out singularColumn))
            {
                Debug.EndTimer();
                Debug.Unindent();

                return new Factorization(matrix, true, singularColumn);
            }

            Debug.EndTimer();
            Debug.Unindent();

            return new Factorization(matrix, false);
        }

        public static bool TryLdlFactorization(Matrix matrix, out int singularColumn)
        {
            Debug.Post("LinearSystems.TryLdlFactorization.");

            if (matrix == null)
                throw new ArgumentNullException("matrix");

            if (!matrix.Squared)
                throw new ArgumentException("The matrix is not squared.", "matrix");

            Debug.Indent();
            Debug.Post("Matrix size: {0}.", matrix.Rows);
            Debug.BeginTimer();

            var result = new Matrix(matrix.Rows, matrix.Columns);

            for (int j = 0; j < result.Columns; j++)
            {
                double sumDiag = 0;

                for (int k = 0; k < j; k++)
                {
                    double ljk = result[j, k];
                    sumDiag += ljk*ljk*result[k, k];
                }

                result[j, j] = matrix[j, j] - sumDiag;

                for (int i = j + 1; i < result.Rows; i++)
                {
                    double sumLij = 0;

                    for (int k = 0; k < j; k++)
                    {
                        double ljk = result[j, k];
                        double lik = result[i, k];

                        sumLij += ljk*lik*result[k, k];
                    }

                    if (Math.Abs(result[j, j]) <= Engine.Epsilon)
                    {
                        singularColumn = j;

                        Debug.EndTimer();
                        Debug.Unindent();

                        return false;
                    }

                    result[i, j] = (matrix[i, j] - sumLij)/result[j, j];
                }
            }

            Debug.EndTimer();
            Debug.Unindent();

            singularColumn = -1;

            return true;
        }

        public static LdlFactorization LdlFactorization(Matrix matrix)
        {
            Debug.Post("LinearSystems.LdlFactorization.");
            Debug.Indent();
            Debug.BeginTimer();

            int singularColumn;

            if (!TryLdlFactorization(matrix, out singularColumn))
            {
                Debug.EndTimer();
                Debug.Unindent();

                return new LdlFactorization(matrix, true, singularColumn);
            }

            Debug.EndTimer();
            Debug.Unindent();

            return new LdlFactorization(matrix, false);
        }

        #endregion

        #region Explicit Solving

        public static bool TryPluSolve(Matrix matrix, Vector vector, PivotStrategy pivotStrategy, out Vector result,
                                       out int singularColumn)
        {
            Debug.Post("LinearSystems.TryPluSolve");

            if (matrix == null)
                throw new ArgumentNullException("matrix");

            if (vector == null)
                throw new ArgumentNullException("vector");

            if (pivotStrategy == null)
                throw new ArgumentNullException("pivotStrategy");

            Debug.Indent();
            Debug.BeginTimer();

            if (!TryPluFactorization(matrix, pivotStrategy, out singularColumn))
            {
                Debug.EndTimer();
                Debug.Unindent();

                result = Vectors.Zeros(matrix.Rows);

                return false;
            }

            Vector y = FowardSubstitution(matrix.LowerDiagonal(), matrix.RowPermutations().Transpose()*vector);
            Vector z = BackSubstitution(matrix.UpperDiagonal(), y);

            Debug.EndTimer();
            Debug.Unindent();

            result = z;

            return true;
        }

        public static LinearSolution PluSolve(Matrix matrix, Vector vector, PivotStrategy pivotStrategy)
        {
            Debug.Post("LinearSystems.PluSolve");
            Debug.Indent();
            Debug.BeginTimer();

            int singularColumn;
            Vector result;

            if (!TryPluSolve(matrix, vector, pivotStrategy, out result, out singularColumn))
            {
                Debug.EndTimer();
                Debug.Unindent();

                return new LinearSolution(result, LinearSolutionType.Singular, singularColumn);
            }

            return new LinearSolution(result, LinearSolutionType.Definite, -1);
        }

        public static LdlSolution LdlSolve(Matrix matrix, Vector vector)
        {
            Debug.Post("LinearSystems.LdlSolve");
            Debug.Indent();
            Debug.BeginTimer();

            int singularColumn;
            Vector result;

            if (!TryLdlSolve(matrix, vector, out result, out singularColumn))
            {
                Debug.EndTimer();
                Debug.Unindent();

                return new LdlSolution(matrix, result, LinearSolutionType.Singular, singularColumn);
            }

            Debug.EndTimer();
            Debug.Unindent();

            return new LdlSolution(matrix, result, LinearSolutionType.Definite, -1);
        }

        public static bool TryLdlSolve(Matrix matrix, Vector vector, out Vector result, out int singularColumn)
        {
            Debug.Post("LinearSystems.TryLdlSolve");

            if (matrix == null)
                throw new ArgumentNullException("matrix");

            if (vector == null)
                throw new ArgumentNullException("vector");

            Debug.Indent();
            Debug.BeginTimer();

            if (!TryLdlFactorization(matrix, out singularColumn))
            {
                Debug.EndTimer();
                Debug.Unindent();

                result = Vectors.Zeros(matrix.Rows);

                return false;
            }

            Matrix lower = matrix.LowerDiagonal();

            Vector y = FowardSubstitution(lower, vector);

            Matrix diagonal = matrix.Diagonal();

            for (int k = 0; k < diagonal.Rows; k++)
            {
                if (Math.Abs(diagonal[k, k]) < Engine.Epsilon)
                {
                    singularColumn = k;
                    result = Vectors.Zeros(matrix.Rows);

                    return false;
                }

                y[k] /= diagonal[k, k];
            }

            Vector z = BackSubstitution(lower.Transpose(), y);

            Debug.EndTimer();
            Debug.Unindent();

            result = z;

            return true;
        }

        public static Vector BackSubstitution(Matrix matrix, Vector vector)
        {
            Debug.Post("LinearSystems.BackSubstitution");

            if (matrix == null)
                throw new ArgumentNullException("matrix");

            if (vector == null)
                throw new ArgumentNullException("vector");

            if (matrix.Columns != vector.Dimension)
                throw new ArgumentException("Dimensions don't match.");

            Debug.Indent();
            Debug.BeginTimer();

            var result = new Vector(vector.Dimension);

            for (int k = result.Dimension - 1; k >= 0; k--)
            {
                if (Math.Abs(matrix[k, k]) < Engine.Epsilon)
                    throw new ArgumentException("The matrix is not invertible. There is a null diagonal value", "matrix");

                double sum = 0;

                for (int j = k + 1; j < result.Dimension; j++)
                    sum += matrix[k, j]*result[j];

                result[k] = (vector[k] - sum)/matrix[k, k];
            }

            Debug.EndTimer();
            Debug.Unindent();

            return result;
        }

        public static Vector FowardSubstitution(Matrix matrix, Vector vector)
        {
            Debug.Post("LinearSystems.FowardSubstitution");

            if (matrix == null)
                throw new ArgumentNullException("matrix");

            if (vector == null)
                throw new ArgumentNullException("vector");

            if (matrix.Columns != vector.Dimension)
                throw new ArgumentException("Dimensions don't match.");

            Debug.Indent();
            Debug.BeginTimer();

            var result = new Vector(vector.Dimension);

            for (int k = 0; k < vector.Dimension; k++)
            {
                if (Math.Abs(matrix[k, k]) < Engine.Epsilon)
                    throw new ArgumentException("The matrix is not invertible. There is a null diagonal value", "matrix");

                double sum = 0;

                for (int j = 0; j < k; j++)
                    sum += matrix[k, j]*result[j];

                result[k] = (vector[k] - sum)/matrix[k, k];
            }

            Debug.EndTimer();
            Debug.Unindent();

            return result;
        }

        #endregion

        #region Pivot Strategies

        public static int SimplePivot(Matrix matrix, int index, int dimension)
        {
            for (int i = index; i < dimension; i++)
                if (Math.Abs(matrix[i, index]) >= Engine.Epsilon)
                    return i;

            return index;
        }

        public static PivotStrategy ScaledPivot(Matrix matrix)
        {
            if (matrix == null)
                throw new ArgumentNullException("matrix");

            var sizes = new double[matrix.Rows];

            for (int i = 0; i < matrix.Rows; i++)
            {
                double s = 0;

                for (int j = 0; j < matrix.Columns; j++)
                    s = Math.Max(s, Math.Abs(matrix[i, j]));

                sizes[i] = s;
            }

            return (m, i, d) =>
                   {
                       if (!ReferenceEquals(m, matrix))
                           throw new ArgumentException(
                               "The matrix used to build this pivot strategy is not the same as 'm'.");

                       int pivot = i;
                       double size = Math.Abs(m[i, i]/sizes[i]);

                       for (int k = i + 1; k < d; k++)
                       {
                           double s = Math.Abs(matrix[k, i]/sizes[k]);

                           if (s > size)
                           {
                               size = s;
                               pivot = k;
                           }
                       }

                       return pivot;
                   };
        }

        #endregion
    }
}
