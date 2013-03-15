using SciSharp.Collections;
using SciSharp.Language.Automata;


namespace SciSharp.Language.Regex
{
    public class RegexConcatNode : RegexBinaryNode
    {
        public RegexConcatNode(IRegexNode left, IRegexNode right)
            : base(left, right) {}

        public override int Priority
        {
            get { return 2; }
        }

        public override FNDADescription Compile()
        {
            FNDADescription left = Left.Compile();
            FNDADescription right = Right.Compile();

            IFNDATransitionFunction function = Automata.Automata.FunctionFromMethod(
                (c, state) =>
                {
                    if (c == '\0' && left.FinalStates.Contains(state))
                        return new ElementSet<int>(right.StartingState + left.States);

                    if (state < left.States)
                        return left.Transitions.Evaluate(c, state);

                    IFiniteSet<int> result = right.Transitions.Evaluate(c, state - left.States);

                    return result == null ? null : result.Displaced(left.States);
                });

            return new FNDADescription(left.States + right.States, right.FinalStates.Displaced(left.States), left.StartingState, function);
        }

        public override IRegexNode Simplify()
        {
            IRegexNode left = Left.Simplify();
            IRegexNode right = Right.Simplify();

            if (left is RegexLiteralNode && right is RegexLiteralNode)
                return new RegexLiteralNode((left as RegexLiteralNode).Literal + (right as RegexLiteralNode).Literal);

            return new RegexConcatNode(left, right);
        }

        public override string ToString()
        {
            string left = Left.Priority <= Priority ? Left.ToString() : string.Format("({0})", Left);
            string right = Right.Priority < Priority ? Right.ToString() : string.Format("({0})", Right);

            return left + right;
        }
    }
}
