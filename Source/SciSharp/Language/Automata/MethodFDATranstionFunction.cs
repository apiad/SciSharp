using System;


namespace SciSharp.Language.Automata
{
    public class MethodFDATranstionFunction : IFDATransitionFunction
    {
        private readonly Func<char, int, int> transitions;

        public MethodFDATranstionFunction(Func<char, int, int> transitions)
        {
            this.transitions = transitions;
        }

        #region IFDATransitionFunction Members

        public int Evaluate(char character, int state)
        {
            return transitions(character, state);
        }

        #endregion
    }
}
