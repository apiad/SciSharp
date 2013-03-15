using System.IO;


namespace SciSharp.Reporting.Tex
{
    public class TextElement : ReportCollection
    {
        private readonly string command;
        private readonly TextElement[] options;

        public TextElement(string command, params TextElement[] options)
        {
            this.command = command;
            this.options = options;
        }

        public void AddContent(TextElement element)
        {
            Elements.Add(element);
        }

        protected override void End(TextWriter output)
        {
            if (Elements.Count > 0)
                output.WriteLine("}");
        }

        protected override void Begin(TextWriter output)
        {
            output.Write("\\{0}", command);

            if (options.Length > 0)
            {
                output.Write("[");
                options[0].Generate(output);

                for (int i = 1; i < options.Length; i++)
                {
                    output.Write(",");
                    options[i].Generate(output);
                }

                output.WriteLine("]");
            }

            if (Elements.Count > 0)
                output.WriteLine("{");
        }
    }
}
