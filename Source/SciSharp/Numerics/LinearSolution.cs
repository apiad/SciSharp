using System;


namespace SciSharp.Numerics
{
    public class LinearSolution
    {
        private readonly int singularColumn;
        private readonly Vector solution;
        private readonly LinearSolutionType type;

        public LinearSolution(Vector solution, LinearSolutionType type)
            : this(solution, type, -1) {}

        public LinearSolution(Vector solution, LinearSolutionType type, int singularColumn)
        {
            if (solution == null)
                throw new ArgumentNullException("solution");

            this.solution = solution;
            this.type = type;
            this.singularColumn = singularColumn;
        }

        public Vector Solution
        {
            get { return solution; }
        }

        public LinearSolutionType Type
        {
            get { return type; }
        }

        public int SingularColumn
        {
            get { return singularColumn; }
        }

        public static bool operator true(LinearSolution solution)
        {
            return solution != null && solution.Type == LinearSolutionType.Definite;
        }

        public static bool operator false(LinearSolution solution)
        {
            return solution == null || solution.Type != LinearSolutionType.Definite;
        }
    }
}
