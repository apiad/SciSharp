using System.Collections.Generic;


namespace SciSharp.Probabilities
{
    public class ComplementEvent : UnaryEvent
    {
        private static readonly Dictionary<Event, ComplementEvent> Singletons =
            new Dictionary<Event, ComplementEvent>();

        private ComplementEvent(Event a)
            : base(a) {}

        protected override bool TryCalculateProbability(ProbabilityFunction p, Event a, out double result, out Formulae formulae)
        {
            Formulae f;

            if (p.TryGetProbability(a, out result, out f))
            {
                result = 1 - result;
                formulae = new Formulae(this, result) {f};
                return true;
            }

            formulae = new Formulae(this, 0);
            return false;
        }

        protected override string ToString(Event a)
        {
            return "!" + a;
        }

        public static Event Get(Event a)
        {
            ComplementEvent result;

            if (Singletons.TryGetValue(a, out result))
                return result;

            if (a is ComplementEvent)
                return ((ComplementEvent) a).A;

            Singletons.Add(a, result = new ComplementEvent(a));
            return result;
        }
    }
}
