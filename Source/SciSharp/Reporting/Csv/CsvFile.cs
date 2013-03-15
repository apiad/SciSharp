using System.IO;


namespace SciSharp.Reporting.Csv
{
    public class CsvFile : Report
    {
        protected override void End(TextWriter output) {}

        protected override void Begin(TextWriter output) {}

        public void Add(CsvMatrix csvMatrix)
        {
            Elements.Add(csvMatrix);
        }
    }
}
