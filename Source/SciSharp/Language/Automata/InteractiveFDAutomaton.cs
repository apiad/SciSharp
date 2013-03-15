using System;
using System.Linq;

using SciSharp.Collections;


namespace SciSharp.Language.Automata
{
    public class InteractiveFDAutomaton : IFDAutomaton
    {
        private readonly IFiniteSet<int> finalStates;
        private readonly int startingState;
        private readonly int states;
        private readonly IFDATransitionFunction transitions;
        private string data;
        private int head;
        private int state;

        public InteractiveFDAutomaton(IFDATransitionFunction transitions, int states, int startingState, params int[] finalStates)
            : this(transitions, states, startingState, new ArraySet<int>(finalStates)) {}

        public InteractiveFDAutomaton(FDADescription fdaDescription)
            : this(fdaDescription.Transitions, fdaDescription.States, fdaDescription.StartingState, fdaDescription.FinalStates)
        {
            if (fdaDescription == null)
                throw new ArgumentNullException("fdaDescription");
        }

        public InteractiveFDAutomaton(IFDATransitionFunction transitions, int states, int startingState, IFiniteSet<int> finalStates)
        {
            if (transitions == null)
                throw new ArgumentNullException("transitions");

            if (finalStates == null)
                throw new ArgumentNullException("finalStates");

            if (states <= 0)
                throw new ArgumentOutOfRangeException("states", "Must be greater than zero.");

            if (startingState < 0 || startingState >= states)
                throw new ArgumentOutOfRangeException("startingState", "Must be a valid state.");

            if (finalStates.Any(i => i < 0 || i >= states))
                throw new ArgumentException("A final state is not a valid state.", "finalStates");

            this.transitions = transitions;
            this.states = states;
            this.startingState = startingState;
            this.finalStates = finalStates;
        }

        #region IFDAutomaton Members

        public bool Step()
        {
            if (data == null)
                throw new InvalidOperationException();

            if (head >= data.Length)
                return false;

            int s = transitions.Evaluate(data[head], state);

            if (s < 0)
                return false;

            if (s >= states)
                throw new InvalidOperationException("The transition function returned an invalid state.");

            state = s;

            return ++head < data.Length;
        }

        public bool Recognized
        {
            get { return data != null && head == data.Length && finalStates.Contains(state); }
        }

        public void Feed(string input)
        {
            data = input;
            head = 0;
            state = startingState;
        }

        public FDADescription Description
        {
            get { return new FDADescription(states, finalStates, startingState, transitions); }
        }

        #endregion
    }
}
