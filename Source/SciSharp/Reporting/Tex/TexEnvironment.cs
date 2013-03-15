using System.IO;


namespace SciSharp.Reporting.Tex
{
    public class TexEnvironment : TextElement
    {
        private readonly string label;
        private readonly string name;
        private readonly bool starred;

        public TexEnvironment(string name, bool starred = false)
            : this(name, null, starred) {}

        public TexEnvironment(string name, string label, bool starred = false)
            : base("\\begin{1}{{0}}".Formatted(name, starred ? "*" : ""))
        {
            this.name = name;
            this.label = label;
            this.starred = starred;
        }

        public string Name
        {
            get { return name; }
        }

        public string Label
        {
            get { return label; }
        }

        public bool Starred
        {
            get { return starred; }
        }

        protected override void End(TextWriter output)
        {
            output.Write("\\end{{0}}".Formatted(name));
        }

        protected override void Begin(TextWriter output)
        {
            if (label != null)
                output.Write("\\label{{0}}".Formatted(label));
        }
    }
}
