using System;
using System.Collections.Generic;


namespace SciSharp.Language.Grammars.Generation
{
    public class LSystemGenerator<T> : Generator<T>
        where T : Node, new()
    {
        public LSystemGenerator(Grammar<T> grammar)
            : base(grammar) {}

        protected override IEnumerable<Token<T>> GenerateSentence(Def<T> symbol)
        {
            throw new NotImplementedException();
        }
    }
}
