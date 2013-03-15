using System.Collections.Generic;


namespace SciSharp.Probabilities
{
    public class IntersectionEvent : BinaryEvent
    {
        private static readonly Dictionary<Pair<Event>, IntersectionEvent> Singletons =
            new Dictionary<Pair<Event>, IntersectionEvent>();

        private IntersectionEvent(Event a, Event b)
            : base(a, b) {}

        protected override bool TryCalculateProbability(Event a, Event b, ProbabilityFunction p, out double result, out Formulae formulae)
        {
            formulae = null;

            double pa, pb, pba, pab;

            if (a.IsIndependent(b) && p.TryGetProbability(a, out pa) && p.TryGetProbability(b, out pb))
            {
                result = pa*pb;
                return true;
            }

            if (a.IsExclusive(b))
            {
                result = 0;
                return true;
            }

            if (a.IsSubset(b) && p.TryGetProbability(a, out pa))
            {
                result = pa;
                return true;
            }

            if (b.IsSubset(a) && p.TryGetProbability(b, out pb))
            {
                result = pb;
                return true;
            }

            if (p.TryGetProbability(a | b, out pab) && p.TryGetProbability(b, out pb))
            {
                result = pb*pab;
                return true;
            }

            if (p.TryGetProbability(b | a, out pba) && p.TryGetProbability(a, out pa))
            {
                result = pa*pba;
                return true;
            }

            result = 0;
            return false;
        }

        protected override string ToString(Event a, Event b)
        {
            return string.Format("({0}.{1})", a, b);
        }

        public override bool IsIndependent(Event other)
        {
            // TODO : Prove that this is true !!!
            return base.IsIndependent(other) || (A.IsIndependent(other) && B.IsIndependent(other));
            // return base.IsIndependent(other);
        }

        public override bool IsExclusive(Event other)
        {
            return base.IsExclusive(other) || (A.IsExclusive(other) && B.IsExclusive(other));
        }

        public override bool IsSubset(Event other)
        {
            return base.IsSubset(other) || (A.IsSubset(other) && B.IsSubset(other));
        }

        public static Event Get(Event a, Event b)
        {
            IntersectionEvent result;

            var ab = new Pair<Event>(a, b);
            var ba = new Pair<Event>(b, a);

            if (Singletons.TryGetValue(ab, out result) || Singletons.TryGetValue(ba, out result))
                return result;

            Singletons.Add(new Pair<Event>(a, b), result = new IntersectionEvent(a, b));

            return result;
        }
    }
}
