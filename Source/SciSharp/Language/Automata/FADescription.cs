using SciSharp.Collections;


namespace SciSharp.Language.Automata
{
    public class FADescription
    {
        protected FADescription(int states, IFiniteSet<int> finalStates, int startingState)
        {
            States = states;
            FinalStates = finalStates;
            StartingState = startingState;
        }

        public int States { get; private set; }
        public IFiniteSet<int> FinalStates { get; private set; }
        public int StartingState { get; private set; }

        public override string ToString()
        {
            return string.Format("States: {0}\nStartingState: {2}\nFinalStates: {1}", States, FinalStates, StartingState);
        }
    }
}
