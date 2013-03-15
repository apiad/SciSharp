using SciSharp.Collections;
using SciSharp.Language.Automata;


namespace SciSharp.Language.Regex
{
    public class RegexClosureNode : RegexUnaryNode
    {
        public RegexClosureNode(IRegexNode node)
            : base(node) {}

        public override int Priority
        {
            get { return 1; }
        }

        public override FNDADescription Compile()
        {
            FNDADescription description = InternalNode.Compile();

            IFNDATransitionFunction function = Automata.Automata.FunctionFromMethod(
                (c, state) =>
                {
                    if (state == description.States && c == '\0')
                        return new ArraySet<int>(description.StartingState, description.States + 1);

                    if (description.FinalStates.Contains(state) && c == '\0')
                        return new ArraySet<int>(description.StartingState, description.States + 1);

                    return description.Transitions.Evaluate(c, state);
                });

            return new FNDADescription(description.States + 2, new ElementSet<int>(description.States + 1), description.States, function);
        }

        public override IRegexNode Simplify()
        {
            IRegexNode node = InternalNode.Simplify();

            if (node is RegexClosureNode)
                return node;

            return new RegexClosureNode(node);
        }

        public override string ToString()
        {
            return InternalNode.Priority <= Priority ? string.Format("{0}*", InternalNode) : string.Format("({0})*", InternalNode);
        }
    }
}
