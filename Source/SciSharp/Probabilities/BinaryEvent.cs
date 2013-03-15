using System;


namespace SciSharp.Probabilities
{
    public abstract class BinaryEvent : Event
    {
        private readonly Event a;
        private readonly Event b;

        protected BinaryEvent(Event a, Event b)
        {
            if (a == null)
                throw new ArgumentNullException("a");

            if (b == null)
                throw new ArgumentNullException("b");

            this.a = a;
            this.b = b;
        }

        public Event A
        {
            get { return a; }
        }

        public Event B
        {
            get { return b; }
        }

        protected internal override bool TryCalculateProbability(ProbabilityFunction p, out double result, out Formulae formulae)
        {
            return TryCalculateProbability(A, B, p, out result, out formulae);
        }

        protected abstract bool TryCalculateProbability(Event a, Event b, ProbabilityFunction p, out double result, out Formulae formulae);

        public override string ToString()
        {
            return ToString(A, B);
        }

        protected abstract string ToString(Event a, Event b);
    }
}
