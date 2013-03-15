using System;


namespace SciSharp.Language
{
    [Serializable]
    public class ParsingException : Exception
    {
        protected ParsingException(string message, Exception innerException)
            : base(message, innerException) {}

        protected ParsingException(string message)
            : base(message) {}

        protected ParsingException() {}
    }
}
