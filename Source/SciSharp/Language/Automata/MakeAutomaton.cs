using System.Collections.Generic;
using System.Linq;


namespace SciSharp.Language.Automata
{
    public static class MakeAutomaton
    {
        public static AutomatonBuilder Recognizing(params char[] alphabet)
        {
            return new AutomatonBuilder(alphabet);
        }

        public static AutomatonBuilder Recognizing(string chars)
        {
            return Recognizing(chars.ToCharArray());
        }

        public static AutomatonBuilder Recognizing(IEnumerable<char> chars)
        {
            return Recognizing(chars.ToArray());
        }

        #region Nested type: AutomatonBuilder

        public class AutomatonBuilder
        {
            private FNDADescription automaton;

            internal AutomatonBuilder(char[] alphabet)
            {
                automaton = FNDADescription.Recognizing(alphabet);
            }

            public AutomatonBuilder StartingWith(params char[] subset)
            {
                automaton = Automata.Intersect(automaton, FNDADescription.StartingWith(subset));
                return this;
            }

            public AutomatonBuilder EndingWith(params char[] subset)
            {
                automaton = Automata.Intersect(automaton, FNDADescription.EndingWith(subset));
                return this;
            }

            public AutomatonBuilder LongerThan(int length)
            {
                automaton = Automata.Intersect(automaton, FNDADescription.LongerThan(length));
                return this;
            }

            public AutomatonBuilder ShorterThan(int length)
            {
                automaton = Automata.Intersect(automaton, FNDADescription.ShorterThan(length));
                return this;
            }

            public AutomatonBuilder WithAtLeast(int count, char c)
            {
                automaton = Automata.Intersect(automaton, FNDADescription.WithAtLeast(count, c));
                return this;
            }

            public AutomatonBuilder WithAtMost(int count, char c)
            {
                automaton = Automata.Intersect(automaton, FNDADescription.WithAtMost(count, c));
                return this;
            }

            public AutomatonBuilder WithExactly(int count, char c)
            {
                automaton = Automata.Intersect(automaton, FNDADescription.WithExactly(count, c));
                return this;
            }

            public static implicit operator FNDADescription(AutomatonBuilder builder)
            {
                return builder.automaton;
            }

            public static implicit operator FDADescription(AutomatonBuilder builder)
            {
                return builder.automaton.ConvertToFDA();
            }
        }

        #endregion
    }
}
