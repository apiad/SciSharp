using System;
using System.Collections.Generic;
using System.Text;


namespace SciSharp.Language.Grammars
{
    [Serializable]
    public class Grammar<T>
        where T : Node, new()
    {
        internal readonly Dictionary<string, Token<T>> ImplicitTokens = new Dictionary<string, Token<T>>();
        internal readonly List<Production<T>> Productions = new List<Production<T>>();
        internal readonly List<Def<T>> Rules = new List<Def<T>>();

        private readonly Token<T> eof;
        private readonly Token<T> epsilon;
        private readonly Token<T> error;
        private readonly List<Token<T>> tokens = new List<Token<T>>();

        public Grammar()
        {
            eof = new Token<T>("") {Name = "$", Grammar = this};
            error = new Token<T>("") {Name = "ERROR", Grammar = this};
            epsilon = new Token<T>("") {Name = "EPSILON", Grammar = this};

            StartSymbol = Rule("S");
            Rules.Add(StartSymbol);
            Builder = new GrammarBuilder<T>(this);
        }

        public Def<T> StartSymbol { get; private set; }

        public Token<T> Epsilon
        {
            get { return epsilon; }
        }

        public Token<T> Eof
        {
            get { return eof; }
        }

        public Token<T> Error
        {
            get { return error; }
        }

        public GrammarBuilder<T> Builder { get; private set; }

        public IEnumerable<Token<T>> Tokens
        {
            get { return tokens; }
        }

        public Def<T> Start(string name)
        {
            StartSymbol.Name = name;
            return StartSymbol;
        }

        public Def<T> Rule()
        {
            return Rule("S" + Rules.Count);
        }

        public Def<T> Rule(string name)
        {
            var rule = new Def<T> {Name = name, Grammar = this};
            Rules.Add(rule);
            return rule;
        }

        internal Token<T> GetImplicitToken(string str)
        {
            Token<T> token;

            if (ImplicitTokens.TryGetValue(str, out token))
                return token;

            token = Token("'" + str + "'", str);
            ImplicitTokens[str] = token;

            return token;
        }

        internal Token<T> GetImplicitToken(char c)
        {
            string str = c.ToString();
            Token<T> token;

            if (ImplicitTokens.TryGetValue(str, out token))
                return token;

            token = Token(c);
            ImplicitTokens[str] = token;

            return token;
        }

        public Token<T> Token(string regex)
        {
            return Token("T" + Rules.Count, regex);
        }

        public Token<T> Token(string name, string regex)
        {
            var token = new Token<T>(regex) {Name = name, Grammar = this};
            tokens.Add(token);
            return token;
        }

        public Token<T> Token(char c)
        {
            return Token("'" + c.ToString() + "'", Escape(c));
        }

        private string Escape(char c)
        {
            switch (c)
            {
                case '+':
                    return "\\+";
                case '-':
                    return "\\-";
                case '*':
                    return "\\*";
                case '\\':
                    return "\\\\";
                case '(':
                    return "\\(";
                case ')':
                    return "\\)";
                case '[':
                    return "\\[";
                case ']':
                    return "\\]";
                case '{':
                    return "\\{";
                case '}':
                    return "\\}";
                case '|':
                    return "\\|";
                default:
                    return c.ToString();
            }
        }

        public Ref<T> Ref()
        {
            return new Ref<T> {Grammar = this};
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var production in Productions)
                sb.AppendLine(production.ToString());

            return sb.ToString();
        }
    }

    public class Grammar : Grammar<BasicNode> {}
}
