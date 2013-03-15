using System;


namespace SciSharp.Language.Automata
{
    public class MappedFDATranstionFunction : IFDATransitionFunction
    {
        private readonly int[,] transitions;

        public MappedFDATranstionFunction(int states)
        {
            transitions = new int[255,states];
        }

        public int this[char character, int state]
        {
            get { return transitions[character, state] + 1; }
            set
            {
                if (value >= transitions.GetLength(1))
                    throw new ArgumentOutOfRangeException("value", "Must be a negative value or a valid state value.");

                transitions[character, state] = value - 1;
            }
        }

        #region IFDATransitionFunction Members

        public int Evaluate(char character, int state)
        {
            return this[character, state];
        }

        #endregion
    }
}
