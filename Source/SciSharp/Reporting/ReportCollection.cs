using System.Collections;
using System.Collections.Generic;
using System.IO;


namespace SciSharp.Reporting
{
    public abstract class ReportCollection : ReportElement, IEnumerable<ReportElement>
    {
        protected List<ReportElement> Elements;

        protected ReportCollection()
        {
            Elements = new List<ReportElement>();
        }

        #region IEnumerable<ReportElement> Members

        public IEnumerator<ReportElement> GetEnumerator()
        {
            return Elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        public override void Generate(TextWriter output)
        {
            Begin(output);

            foreach (ReportElement reportElement in Elements)
                reportElement.Generate(output);

            End(output);
        }

        protected abstract void End(TextWriter output);

        protected abstract void Begin(TextWriter output);
    }
}
