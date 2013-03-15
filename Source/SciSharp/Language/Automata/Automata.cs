using System;
using System.Collections.Generic;
using System.Linq;

using SciSharp.Collections;


namespace SciSharp.Language.Automata
{
    public static class Automata
    {
        public static bool Process(this IAutomaton automaton, string input)
        {
            automaton.Feed(input);
            while (automaton.Step()) {}
            return automaton.Recognized;
        }

        public static IFDATransitionFunction FunctionFromMethod(Func<char, int, int> transitions)
        {
            return new MethodFDATranstionFunction(transitions);
        }

        public static IFNDATransitionFunction FunctionFromMethod(Func<char, int, IFiniteSet<int>> transitions)
        {
            return new MethodFNDATransitionFunction(transitions);
        }

        public static FDADescription ConvertToFDA(this FNDADescription automatonDescription)
        {
            if (automatonDescription == null)
                throw new ArgumentNullException("automatonDescription");

            throw new NotImplementedException();
        }

        public static IFiniteSet<int> Move(this FNDADescription fnda, IFiniteSet<int> states, char c)
        {
            var mark = new bool[fnda.States];
            var q = new Queue<int>(fnda.States);
            var list = new LinkedList<int>();

            foreach (int state in states)
                q.Enqueue(state);

            while (q.Count > 0)
            {
                int s = q.Dequeue();
                mark[s] = true;

                foreach (int i in fnda.Transitions.Evaluate(c, s))
                    if (!mark[i])
                        q.Enqueue(i);

                list.AddLast(s);
            }

            return new ArraySet<int>(list.ToArray());
        }

        public static IFiniteSet<int> EpsilonClosure(this FNDADescription fnda, IFiniteSet<int> states)
        {
            return Move(fnda, states, '\0');
        }

        public static IFiniteSet<int> EpsilonMove(this FNDADescription fnda, IFiniteSet<int> states, char c)
        {
            return EpsilonClosure(fnda, Move(fnda, states, c));
        }

        public static FNDADescription Intersect(FNDADescription description1, FNDADescription description2)
        {
            throw new NotImplementedException();
        }
    }
}
