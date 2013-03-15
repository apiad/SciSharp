using System;
using System.IO;


namespace SciSharp
{
    public static class Logger
    {
        private static TextWriter writer;

        static Logger()
        {
            Writer = TextWriter.Null;
        }

        public static TextWriter Writer
        {
            get { return writer; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                writer = value;
            }
        }

        public static void Log(string message, params object[] args)
        {
            Writer.WriteLine(message, args);
        }

        public static IContext OpenContext()
        {
            return new LoggerContext(writer);
        }

        #region Nested type: LoggerContext

        private class LoggerContext : Context
        {
            private readonly TextWriter previousWritter;

            public LoggerContext(TextWriter previousWritter)
            {
                this.previousWritter = previousWritter;
            }

            protected override void SafeEnd()
            {
                Writer = previousWritter;
            }
        }

        #endregion
    }
}
