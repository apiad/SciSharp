namespace SciSharp.Numerics
{
    public class LdlFactorization : Factorization
    {
        private Matrix diagonal;
        private Vector eigenvalues;
        private Matrix lower;

        public LdlFactorization(Matrix matrix, bool singular)
            : base(matrix, singular)
        {
            Initialize(matrix);
        }

        public LdlFactorization(Matrix matrix, bool singular, int singularColumn)
            : base(matrix, singular, singularColumn)
        {
            Initialize(matrix);
        }

        public Matrix Lower
        {
            get { return lower; }
        }

        public Matrix Diagonal
        {
            get { return diagonal; }
        }

        public Vector Eigenvalues
        {
            get { return eigenvalues; }
        }

        private void Initialize(Matrix matrix)
        {
            lower = matrix.LowerDiagonal();
            diagonal = matrix.Diagonal();

            var v = new Vector(diagonal.Rows);

            for (int i = 0; i < v.Dimension; i++)
                v[i] = diagonal[i, i];

            eigenvalues = v;
        }
    }
}
