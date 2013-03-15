using System;


namespace SciSharp.Language.Grammars
{
    [Serializable]
    public class NotMatchedException : ParsingException
    {
        public NotMatchedException()
            : this(string.Empty) {}

        public NotMatchedException(string expression)
            : this(expression, string.Empty) {}

        public NotMatchedException(string expression, string message)
            : this(expression, message, null) {}

        public NotMatchedException(string expression, string message, Exception innerException)
            : base(message, innerException)
        {
            Expression = expression;
        }

        public string Expression { get; private set; }

        public override string ToString()
        {
            return string.Format("The expression {0} could not be matched.", Expression);
        }
    }
}
