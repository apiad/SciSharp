namespace SciSharp.Benchmarks.Combinatorial
{
    public abstract class ProblemSolution
    {
        public ProblemSolution(ProblemInstance instance) { }

        public abstract void Load(string file);

        public abstract bool IsFeasible();

        public abstract double Fitness();
    }
}
