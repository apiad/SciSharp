using System;

using SciSharp.Collections;


namespace SciSharp.Language.Automata
{
    internal class MethodFNDATransitionFunction : IFNDATransitionFunction
    {
        private readonly Func<char, int, IFiniteSet<int>> transitions;

        public MethodFNDATransitionFunction(Func<char, int, IFiniteSet<int>> transitions)
        {
            if (transitions == null)
                throw new ArgumentNullException("transitions");

            this.transitions = transitions;
        }

        #region IFNDATransitionFunction Members

        public IFiniteSet<int> Evaluate(char character, int state)
        {
            return transitions(character, state);
        }

        #endregion
    }
}
