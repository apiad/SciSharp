using System.Collections.Generic;


namespace SciSharp.Language.Grammars.Generation
{
    public abstract class Generator<T>
        where T : Node, new()
    {
        protected readonly Grammar<T> Grammar;

        protected Generator(Grammar<T> grammar)
        {
            Grammar = grammar;
        }

        public IEnumerable<T> Generate()
        {
            Grammar.StartSymbol.Node = new T();

            foreach (var token in GenerateSentence(Grammar.StartSymbol))
                yield return token.Node;
        }

        protected abstract IEnumerable<Token<T>> GenerateSentence(Def<T> symbol);
    }
}
