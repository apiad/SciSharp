using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;


namespace SciSharp.Language.Grammars
{
    public class TokenStream<T> : IDisposable
        where T : Node, new()
    {
        private readonly Grammar<T> grammar;
        private readonly System.Text.RegularExpressions.Regex splitter;
        private readonly TextReader stream;
        private readonly Token<T>[] tokens;
        private int currentIndex;
        private string[] currentLine;

        public TokenStream(Stream stream, Grammar<T> grammar)
        {
            this.grammar = grammar;
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (grammar == null)
                throw new ArgumentNullException("grammar");

            this.stream = new StreamReader(stream);
            tokens = grammar.Tokens.ToArray();
            splitter = BuildSplitter();
        }

        public TokenStream(TextReader reader, Grammar<T> grammar)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");
            if (grammar == null)
                throw new ArgumentNullException("grammar");

            stream = reader;
            this.grammar = grammar;
            tokens = grammar.Tokens.ToArray();
            splitter = BuildSplitter();
        }

        public TokenStream(string path, Grammar<T> grammar)
        {
            if (path == null)
                throw new ArgumentNullException("path");
            if (grammar == null)
                throw new ArgumentNullException("grammar");

            this.grammar = grammar;
            stream = new StreamReader(path);
            tokens = grammar.Tokens.ToArray();
            splitter = BuildSplitter();
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        private System.Text.RegularExpressions.Regex BuildSplitter()
        {
            string regex = string.Format("({0})", string.Join("|", tokens.Select(t => "(" + t.Regex + ")").ToArray()));
            return new System.Text.RegularExpressions.Regex(regex, RegexOptions.Compiled | RegexOptions.ExplicitCapture);
        }

        private string NextString()
        {
            while (currentLine == null || currentIndex >= currentLine.Length)
            {
                if (stream.Peek() == -1)
                    return null;

                string line = stream.ReadLine();

                if (line == null)
                    return null;

                // Match a todos los tokens de la linea
                MatchCollection matches = splitter.Matches(line);

                // Crear el nuevo Array de String
                currentLine = new string[matches.Count];

                // Copiar los matches
                for (int i = 0; i < matches.Count; i++)
                    currentLine[i] = matches[i].Value;

                currentIndex = 0;
            }

            return currentLine[currentIndex++];
        }

        public Token<T> NextToken()
        {
            string str = NextString();

            if (str == null)
                return grammar.Eof;

            foreach (var tokenRule in tokens)
            {
                Match match = tokenRule.Match(str);

                if (match.Success && match.Value == str)
                {
                    tokenRule.Node = new T {Match = str};
                    return tokenRule;
                }
            }

            return grammar.Error;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                stream.Dispose();
        }
    }
}
