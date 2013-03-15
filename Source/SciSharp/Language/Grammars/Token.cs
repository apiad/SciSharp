using System;
using System.Text.RegularExpressions;


namespace SciSharp.Language.Grammars
{
    [Serializable]
    public class Token<T> : Def<T>
        where T : Node, new()
    {
        private readonly System.Text.RegularExpressions.Regex regex;
        private readonly String regexStr;

        public Token(string regex)
        {
            if (regex == null)
                throw new ArgumentNullException("regex");

            regexStr = regex;
            this.regex = new System.Text.RegularExpressions.Regex(regex, RegexOptions.Compiled | RegexOptions.ExplicitCapture);
        }

        public override bool IsNonTerminal
        {
            get { return false; }
        }

        public string Regex
        {
            get { return regexStr; }
        }

        internal Match Match(string str)
        {
            return regex.Match(str);
        }
    }
}
