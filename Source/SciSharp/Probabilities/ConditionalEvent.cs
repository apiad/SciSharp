using System.Collections.Generic;


namespace SciSharp.Probabilities
{
    public class ConditionalEvent : BinaryEvent
    {
        private static readonly Dictionary<Pair<Event>, ConditionalEvent> Singletons =
            new Dictionary<Pair<Event>, ConditionalEvent>();

        private ConditionalEvent(Event a, Event b)
            : base(a, b) {}

        protected override bool TryCalculateProbability(Event a, Event b, ProbabilityFunction p, out double result, out Formulae formulae)
        {
            double pab, pb;
            Formulae fab, fb;

            bool ok1 = p.TryGetProbability(a*b, out pab, out fab);
            bool ok2 = p.TryGetProbability(b, out pb, out fb);

            result = pab/pb;
            formulae = new Formulae(this, result) {fab, fb};

            if (ok1 && ok2) return true;

            double pba, pa;
            Formulae fba, fa;

            ok1 = p.TryGetProbability(b | a, out pba, out fba);
            ok2 = p.TryGetProbability(a, out pa, out fa);
            bool ok3 = p.TryGetProbability(b, out pb, out fb);

            result = pba*pa/pb;
            formulae = new Formulae(this, result) {fba, fa, fb};

            return ok1 && ok2 && ok3;
        }

        protected override string ToString(Event a, Event b)
        {
            return string.Format("({0}|{1})", a, b);
        }

        public static Event Get(Event a, Event b)
        {
            ConditionalEvent result;

            var ab = new Pair<Event>(a, b);

            if (Singletons.TryGetValue(ab, out result))
                return result;

            Singletons.Add(new Pair<Event>(a, b), result = new ConditionalEvent(a, b));

            return result;
        }
    }
}
