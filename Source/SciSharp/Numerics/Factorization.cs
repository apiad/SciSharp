namespace SciSharp.Numerics
{
    /// <summary>
    /// Represents the result of a factorization algorithm for matrices.
    /// </summary>
    public class Factorization
    {
        private readonly Matrix matrix;
        private readonly bool singular;
        private readonly int singularColumn;

        public Factorization(Matrix matrix, bool singular)
            : this(matrix, singular, -1) {}

        public Factorization(Matrix matrix, bool singular, int singularColumn)
        {
            this.matrix = matrix;
            this.singular = singular;
            this.singularColumn = singularColumn;
        }

        /// <summary>
        /// Gets the factorized matrix.
        /// </summary>
        public Matrix Matrix
        {
            get { return matrix; }
        }

        public bool Singular
        {
            get { return singular; }
        }

        public int SingularColumn
        {
            get { return singularColumn; }
        }

        public static bool operator true(Factorization factorization)
        {
            return factorization != null && !factorization.Singular;
        }

        public static bool operator false(Factorization factorization)
        {
            return factorization == null || factorization.Singular;
        }
    }
}
