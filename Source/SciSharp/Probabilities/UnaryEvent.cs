namespace SciSharp.Probabilities
{
    public abstract class UnaryEvent : Event
    {
        private readonly Event a;

        protected UnaryEvent(Event a)
        {
            this.a = a;
        }

        public Event A
        {
            get { return a; }
        }

        protected internal override bool TryCalculateProbability(ProbabilityFunction p, out double result, out Formulae formulae)
        {
            return TryCalculateProbability(p, A, out result, out formulae);
        }

        protected abstract bool TryCalculateProbability(ProbabilityFunction p, Event a, out double result, out Formulae formulae);

        public override string ToString()
        {
            return ToString(A);
        }

        protected abstract string ToString(Event a);
    }
}
