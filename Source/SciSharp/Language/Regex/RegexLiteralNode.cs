using System;

using SciSharp.Collections;
using SciSharp.Language.Automata;


namespace SciSharp.Language.Regex
{
    public class RegexLiteralNode : RegexLeafNode
    {
        private readonly string literal;

        public RegexLiteralNode(string literal)
        {
            if (literal == null)
                throw new ArgumentNullException("literal");

            this.literal = literal;
        }

        public string Literal
        {
            get { return literal; }
        }

        public override int Priority
        {
            get { return 0; }
        }

        public override FNDADescription Compile()
        {
            IFNDATransitionFunction function = Automata.Automata.FunctionFromMethod(
                (c, state) =>
                {
                    if (state >= 0 && state < literal.Length && literal[state] == c)
                        return new ElementSet<int>(state + 1);

                    return null;
                });

            return new FNDADescription(literal.Length + 1, new ElementSet<int>(literal.Length), 0, function);
        }

        public override IRegexNode Simplify()
        {
            return new RegexLiteralNode(Literal);
        }

        public override string ToString()
        {
            return Literal;
        }
    }
}
