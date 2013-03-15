using SciSharp.Collections;
using SciSharp.Language.Automata;


namespace SciSharp.Language.Regex
{
    public class RegexUnionNode : RegexBinaryNode
    {
        public RegexUnionNode(IRegexNode left, IRegexNode right)
            : base(left, right) {}

        public override int Priority
        {
            get { return 3; }
        }

        public override FNDADescription Compile()
        {
            FNDADescription left = Left.Compile();
            FNDADescription righ = Right.Compile();

            IFNDATransitionFunction function = Automata.Automata.FunctionFromMethod(
                (c, state) =>
                {
                    if (state == left.States + righ.States && c == '\0')
                        return new ArraySet<int>(left.StartingState, righ.StartingState + left.States);

                    if ((left.FinalStates.Contains(state) || righ.FinalStates.Contains(state - left.States)) && c == '\0')
                        return new ElementSet<int>(left.States + righ.States + 1);

                    if (state < left.States)
                        return left.Transitions.Evaluate(c, state);

                    IFiniteSet<int> result = righ.Transitions.Evaluate(c, state - left.States);

                    return result == null ? null : result.Displaced(left.States);
                });

            return new FNDADescription(left.States + righ.States + 2, new ElementSet<int>(left.States + righ.States + 1), left.States + righ.States, function);
        }

        public override IRegexNode Simplify()
        {
            return new RegexUnionNode(Left.Simplify(), Right.Simplify());
        }

        public override string ToString()
        {
            string left = Left.Priority <= Priority ? Left.ToString() : string.Format("({0})", Left);
            string right = Right.Priority < Priority ? Right.ToString() : string.Format("({0})", Right);

            return string.Format("{0}+{1}", left, right);
        }
    }
}
