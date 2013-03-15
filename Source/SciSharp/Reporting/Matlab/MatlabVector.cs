using System.IO;


namespace SciSharp.Reporting.Matlab
{
    public class MatlabVector : MatlabElement
    {
        private readonly string description;
        private readonly string name;
        private readonly Vector vector;

        public MatlabVector(Vector vector, string name, string description = null)
        {
            this.vector = vector;
            this.name = name;
            this.description = description;
        }

        public override void Generate(TextWriter output)
        {
            output.WriteLine("% {0}", description ?? "Variable " + name);
            output.Write("{0} = [", name);

            for (int i = 0; i < vector.Dimension; i++)
                output.Write(" {0} ", vector[i]);

            output.WriteLine("];\n");
        }
    }
}
