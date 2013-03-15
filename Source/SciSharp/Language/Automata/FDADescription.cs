using System.Text;

using SciSharp.Collections;


namespace SciSharp.Language.Automata
{
    public class FDADescription : FADescription
    {
        public FDADescription(int states, IFiniteSet<int> finalStates, int startingState, IFDATransitionFunction transitions)
            : base(states, finalStates, startingState)
        {
            Transitions = transitions;
        }

        public IFDATransitionFunction Transitions { get; private set; }

        public override string ToString()
        {
            var s = new StringBuilder(base.ToString() + "\nTransitions:\n");

            for (int i = 0; i < States; i++)
            {
                for (char c = char.MinValue; c <= char.MaxValue; c++)
                {
                    int result = Transitions.Evaluate(c, i);
                    if (result >= 0 && result < States)
                    {
                        s.AppendFormat("({0}\t,{1}\t) => {2}\n", i, c == '\0' ? (object) "eps" : c, result);
                    }
                }
            }

            return s.ToString();
        }
    }
}
