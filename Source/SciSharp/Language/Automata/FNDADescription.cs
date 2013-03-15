using System;
using System.Text;

using SciSharp.Collections;


namespace SciSharp.Language.Automata
{
    public class FNDADescription : FADescription
    {
        public FNDADescription(int states, IFiniteSet<int> finalStates, int startingState, IFNDATransitionFunction transitions)
            : base(states, finalStates, startingState)
        {
            Transitions = transitions;
        }

        public IFNDATransitionFunction Transitions { get; private set; }

        public override string ToString()
        {
            var s = new StringBuilder(base.ToString() + "\nTransitions:\n");

            for (int i = 0; i < States; i++)
            {
                for (var c = (char) 0; c <= 255; c++)
                {
                    IFiniteSet<int> result = Transitions.Evaluate(c, i);
                    if (result != null)
                    {
                        s.AppendFormat("({0}\t,{1}\t) => {2}\n", i, c == '\0' ? (object) "eps" : c, result);
                    }
                }
            }

            return s.ToString();
        }

        public static FNDADescription Recognizing(char[] alphabet)
        {
            throw new NotImplementedException();
        }

        public static FNDADescription StartingWith(char[] subset)
        {
            throw new NotImplementedException();
        }

        public static FNDADescription EndingWith(char[] subset)
        {
            throw new NotImplementedException();
        }

        public static FNDADescription LongerThan(int length)
        {
            throw new NotImplementedException();
        }

        public static FNDADescription ShorterThan(int length)
        {
            throw new NotImplementedException();
        }

        public static FNDADescription WithAtLeast(int count, char c)
        {
            throw new NotImplementedException();
        }

        public static FNDADescription WithAtMost(int count, char c)
        {
            throw new NotImplementedException();
        }

        public static FNDADescription WithExactly(int count, char c)
        {
            throw new NotImplementedException();
        }
    }
}
