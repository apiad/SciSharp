using System;
using System.IO;
using System.Text;


namespace SciSharp.Tools
{
    public class MultiWriter : TextWriter
    {
        private readonly TextWriter[] writers;

        public MultiWriter(params TextWriter[] writers)
        {
            if (writers == null)
                throw new ArgumentNullException("writers");

            this.writers = writers;
        }

        public override Encoding Encoding
        {
            get { return writers[0].Encoding; }
        }

        public override void Close()
        {
            foreach (TextWriter textWriter in writers)
                textWriter.Close();
        }

        public override void Flush()
        {
            foreach (TextWriter textWriter in writers)
                textWriter.Flush();
        }

        public override void Write(bool value)
        {
            foreach (TextWriter textWriter in writers)
                textWriter.Write(value);
        }

        public override void Write(char value)
        {
            foreach (TextWriter textWriter in writers)
                textWriter.Write(value);
        }

        public override void Write(char[] buffer)
        {
            foreach (TextWriter textWriter in writers)
                textWriter.Write(buffer);
        }

        public override void Write(char[] buffer, int index, int count)
        {
            foreach (TextWriter textWriter in writers)
                textWriter.Write(buffer, index, count);
        }

        public override void Write(decimal value)
        {
            foreach (TextWriter textWriter in writers)
                textWriter.Write(value);
        }

        public override void Write(double value)
        {
            foreach (TextWriter textWriter in writers)
                textWriter.Write(value);
        }

        public override void Write(float value)
        {
            foreach (TextWriter textWriter in writers)
                textWriter.Write(value);
        }

        public override void Write(int value)
        {
            foreach (TextWriter textWriter in writers)
                textWriter.Write(value);
        }

        public override void Write(long value)
        {
            foreach (TextWriter textWriter in writers)
                textWriter.Write(value);
        }

        public override void Write(object value)
        {
            foreach (TextWriter textWriter in writers)
                textWriter.Write(value);
        }

        public override void Write(string format, object arg0)
        {
            foreach (TextWriter textWriter in writers)
                textWriter.Write(format, arg0);
        }

        public override void Write(string format, object arg0, object arg1)
        {
            foreach (TextWriter textWriter in writers)
                textWriter.Write(format, arg0, arg1);
        }

        public override void Write(string format, object arg0, object arg1, object arg2)
        {
            foreach (TextWriter textWriter in writers)
                textWriter.Write(format, arg0, arg1, arg2);
        }

        public override void Write(string format, params object[] arg)
        {
            foreach (TextWriter textWriter in writers)
                textWriter.Write(format, arg);
        }

        public override void Write(string value)
        {
            foreach (TextWriter textWriter in writers)
                textWriter.Write(value);
        }

        public override void Write(uint value)
        {
            foreach (TextWriter textWriter in writers)
                textWriter.Write(value);
        }

        public override void Write(ulong value)
        {
            foreach (TextWriter textWriter in writers)
                textWriter.Write(value);
        }

        public override void WriteLine()
        {
            foreach (TextWriter textWriter in writers)
                textWriter.WriteLine();
        }

        public override void WriteLine(bool value)
        {
            foreach (TextWriter textWriter in writers)
                textWriter.WriteLine(value);
        }

        public override void WriteLine(char value)
        {
            foreach (TextWriter textWriter in writers)
                textWriter.WriteLine(value);
        }

        public override void WriteLine(char[] buffer)
        {
            foreach (TextWriter textWriter in writers)
                textWriter.WriteLine(buffer);
        }

        public override void WriteLine(char[] buffer, int index, int count)
        {
            foreach (TextWriter textWriter in writers)
                textWriter.WriteLine(buffer, index, count);
        }

        public override void WriteLine(decimal value)
        {
            foreach (TextWriter textWriter in writers)
                textWriter.WriteLine(value);
        }

        public override void WriteLine(double value)
        {
            foreach (TextWriter textWriter in writers)
                textWriter.WriteLine(value);
        }

        public override void WriteLine(float value)
        {
            foreach (TextWriter textWriter in writers)
                textWriter.WriteLine(value);
        }

        public override void WriteLine(int value)
        {
            foreach (TextWriter textWriter in writers)
                textWriter.WriteLine(value);
        }

        public override void WriteLine(long value)
        {
            foreach (TextWriter textWriter in writers)
                textWriter.WriteLine(value);
        }

        public override void WriteLine(object value)
        {
            foreach (TextWriter textWriter in writers)
                textWriter.WriteLine(value);
        }

        public override void WriteLine(string format, object arg0)
        {
            foreach (TextWriter textWriter in writers)
                textWriter.WriteLine(format, arg0);
        }

        public override void WriteLine(string format, object arg0, object arg1)
        {
            foreach (TextWriter textWriter in writers)
                textWriter.WriteLine(format, arg0, arg1);
        }

        public override void WriteLine(string format, object arg0, object arg1, object arg2)
        {
            foreach (TextWriter textWriter in writers)
                textWriter.WriteLine(format, arg0, arg1, arg2);
        }

        public override void WriteLine(string format, params object[] arg)
        {
            foreach (TextWriter textWriter in writers)
                textWriter.WriteLine(format, arg);
        }

        public override void WriteLine(string value)
        {
            foreach (TextWriter textWriter in writers)
                textWriter.WriteLine(value);
        }

        public override void WriteLine(uint value)
        {
            foreach (TextWriter textWriter in writers)
                textWriter.WriteLine(value);
        }

        public override void WriteLine(ulong value)
        {
            foreach (TextWriter textWriter in writers)
                textWriter.WriteLine(value);
        }

        protected override void Dispose(bool disposing)
        {
            foreach (var textWriter in writers)
                textWriter.Dispose();
        }

        public override string NewLine
        {
            get
            {
                return base.NewLine;
            }
            set
            {
                base.NewLine = value;

                foreach (var textWriter in writers)
                    textWriter.NewLine = value;
            }
        }
    }
}
