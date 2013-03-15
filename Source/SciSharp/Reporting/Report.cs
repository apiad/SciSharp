using System;
using System.IO;


namespace SciSharp.Reporting
{
    public abstract class Report : ReportCollection
    {
        public static bool OverwriteExistingFiles { get; set; }

        public string Generate()
        {
            using (var output = new StringWriter())
            {
                Generate(output);
                return output.GetStringBuilder().ToString();
            }
        }

        public void Generate(string filename)
        {
            if (File.Exists(filename) && !OverwriteExistingFiles)
                throw new InvalidOperationException("The file already exists. Set " +
                                                    "`Report.OverwriteExistingFiles=true` if you intended to overwrite.");

            using (var output = new StreamWriter(filename))
                Generate(output);
        }
    }
}
