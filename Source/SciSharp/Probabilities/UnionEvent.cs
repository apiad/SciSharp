using System.Collections.Generic;


namespace SciSharp.Probabilities
{
    public class UnionEvent : BinaryEvent
    {
        private static readonly Dictionary<Pair<Event>, UnionEvent> Singletons =
            new Dictionary<Pair<Event>, UnionEvent>();

        private UnionEvent(Event a, Event b)
            : base(a, b) {}

        protected override bool TryCalculateProbability(Event a, Event b, ProbabilityFunction p, out double result, out Formulae formulae)
        {
            formulae = null;

            double pa, pb, pab;

            if (a.IsSubset(b) && p.TryGetProbability(b, out pb))
            {
                result = pb;
                return true;
            }

            if (b.IsSubset(a) && p.TryGetProbability(a, out pa))
            {
                result = pa;
                return true;
            }

            if (p.TryGetProbability(a, out pa) && p.TryGetProbability(b, out pb) && p.TryGetProbability(a*b, out pab))
            {
                result = pa + pb - pab;
                return true;
            }

            result = 0;
            return false;
        }

        public override bool IsExclusive(Event other)
        {
            return base.IsExclusive(other) || (A.IsExclusive(other) && B.IsExclusive(other));
        }

        public override bool IsIndependent(Event other)
        {
            return base.IsIndependent(other) || (A.IsIndependent(other) && B.IsIndependent(other));
        }

        public override bool IsSubset(Event other)
        {
            return base.IsSubset(other) || (A.IsSubset(other) && B.IsSubset(other));
        }

        public override bool IsSuperset(Event other)
        {
            return base.IsSuperset(other) || (A.IsSuperset(other) && B.IsSuperset(other));
        }

        protected override string ToString(Event a, Event b)
        {
            return string.Format("({0}+{1})", a, b);
        }

        public static Event Get(Event a, Event b)
        {
            UnionEvent result;

            var ab = new Pair<Event>(a, b);
            var ba = new Pair<Event>(b, a);

            if (Singletons.TryGetValue(ab, out result) || Singletons.TryGetValue(ba, out result))
                return result;

            Singletons.Add(new Pair<Event>(a, b), result = new UnionEvent(a, b));

            return result;
        }
    }
}
