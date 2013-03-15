using System.IO;


namespace SciSharp.Reporting.Csv
{
    public class CsvMatrix : ReportElement
    {
        private readonly Matrix matrix;

        public CsvMatrix(Matrix matrix)
        {
            this.matrix = matrix;
        }

        public override void Generate(TextWriter output)
        {
            int rows = matrix.Rows;
            int cols = matrix.Columns;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                    output.Write("{0}, ", matrix[i, j]);

                output.WriteLine();
            }
        }
    }
}
