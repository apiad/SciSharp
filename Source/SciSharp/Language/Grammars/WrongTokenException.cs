using System;
using System.Text;


namespace SciSharp.Language.Grammars
{
    [Serializable]
    public class WrongTokenException<T> : ParsingException
    {
        public WrongTokenException(T currentToken, params T[] expectedTokens)
        {
            ExpectedTokens = expectedTokens;
            CurrentToken = currentToken;
        }

        public T[] ExpectedTokens { get; private set; }
        public T CurrentToken { get; private set; }

        public override string ToString()
        {
            var s = new StringBuilder();

            for (int i = 0; i < ExpectedTokens.Length - 1; i++)
                s.Append(ExpectedTokens[i] + ", ");

            if (ExpectedTokens.Length > 0)
                s.Append(ExpectedTokens[ExpectedTokens.Length - 1]);

            return string.Format("Found token {0} while expecting one of the following tokens: {1}.", CurrentToken, s);
        }
    }
}
