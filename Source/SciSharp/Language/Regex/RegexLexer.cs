using System;


namespace SciSharp.Language.Regex
{
    public class RegexLexer : ILexer<RegexToken>
    {
        private readonly string input;
        private int pos;

        public RegexLexer(string input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            this.input = input;
        }

        #region ILexer<RegexToken> Members

        public RegexToken NextToken()
        {
            while (pos < input.Length && input[pos] == ' ')
                pos++;

            if (pos < input.Length)
            {
                char c;
                switch (c = input[pos++])
                {
                    case '+':
                        return new RegexToken(RegexTokenType.Union);
                    case '(':
                        return new RegexToken(RegexTokenType.OpenBracket);
                    case ')':
                        return new RegexToken(RegexTokenType.CloseBracket);
                    case '*':
                        return new RegexToken(RegexTokenType.Clousure);
                    default:
                        return new RegexToken(RegexTokenType.Literal, c);
                }
            }

            return new RegexToken(RegexTokenType.End);
        }

        #endregion
    }
}
