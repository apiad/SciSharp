namespace SciSharp.Numerics
{
    public class LdlSolution : LinearSolution
    {
        private Matrix diagonal;
        private Vector eigenvalues;
        private Matrix lower;

        public LdlSolution(Matrix matrix, Vector solution, LinearSolutionType type)
            : base(solution, type)
        {
            Initialize(matrix);
        }

        public LdlSolution(Matrix matrix, Vector solution, LinearSolutionType type, int singularColumn)
            : base(solution, type, singularColumn)
        {
            Initialize(matrix);
        }

        public Vector Eigenvalues
        {
            get { return eigenvalues; }
        }

        public Matrix Lower
        {
            get { return lower; }
        }

        public Matrix Diagonal
        {
            get { return diagonal; }
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
