using System.IO;


namespace SciSharp.Reporting
{
    public abstract class ReportElement
    {
        public abstract void Generate(TextWriter output);
    }
}
