using System;
using System.Collections.Generic;


namespace SciSharp.Probabilities
{
    public abstract class Event : IEquatable<Event>
    {
        #region Instance Fields

        private readonly HashSet<Event> exclusives = new HashSet<Event>();
        private readonly HashSet<Event> independents = new HashSet<Event>();
        private readonly HashSet<Event> subsets = new HashSet<Event>();
        private readonly HashSet<Event> supersets = new HashSet<Event>();

        #endregion

        #region IEquatable<Event> Members

        public bool Equals(Event other)
        {
            if (ReferenceEquals(null, other))
                return false;

            return ReferenceEquals(this, other);
        }

        #endregion

        #region Members

        public void MakeIndependent(Event other)
        {
            if (IsExclusive(other))
                throw new ArgumentException(string.Format("Events {0} and {1} are {2}.", this, other, "exclusives"));

            independents.Add(other);
            other.independents.Add(this);
        }

        public static void MakeIndependent(params Event[] events)
        {
            for (int i = 0; i < events.Length - 1; i++)
                for (int j = i + 1; j < events.Length; j++)
                    events[i].MakeIndependent(events[j]);
        }

        public void MakeExclusive(Event other)
        {
            if (IsIndependent(other))
                throw new ArgumentException(string.Format("Events {0} and {1} are {2}.", this, other, "independents"));

            exclusives.Add(other);
            other.exclusives.Add(this);
        }

        public static void MakeExclusive(params Event[] events)
        {
            for (int i = 0; i < events.Length - 1; i++)
                for (int j = i + 1; j < events.Length; j++)
                    events[i].MakeExclusive(events[j]);
        }

        public void MakeSubset(Event other)
        {
            if (IsExclusive(other))
                throw new ArgumentException(string.Format("Events {0} and {1} are {2}.", this, other, "exclusives"));

            other.subsets.Add(this);
            supersets.Add(other);
        }

        public void MakeSuperset(Event other)
        {
            other.MakeSubset(this);
        }

        public virtual bool IsIndependent(Event other)
        {
            return IsRelated(other, x => x.independents);
        }

        private bool IsRelated(Event other, Func<Event, HashSet<Event>> relation)
        {
            var q = new Queue<Event>();
            var mark = new HashSet<Event>();

            q.Enqueue(this);

            while (q.Count > 0)
            {
                Event ev = q.Dequeue();
                HashSet<Event> set = relation(ev);

                if (set.Contains(other))
                {
                    relation(this).Add(other);
                    return true;
                }

                mark.Add(ev);

                foreach (Event e in set)
                    if (!mark.Contains(e))
                        q.Enqueue(e);
            }

            return false;
        }

        public virtual bool IsExclusive(Event other)
        {
            return IsRelated(other, x => x.exclusives);
        }

        public virtual bool IsSubset(Event other)
        {
            return IsRelated(other, x => x.subsets);
        }

        public virtual bool IsSuperset(Event other)
        {
            return IsRelated(other, x => x.supersets);
        }

        protected internal abstract bool TryCalculateProbability(ProbabilityFunction p, out double result, out Formulae formulae);

        public static bool operator ==(Event left, Event right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Event left, Event right)
        {
            return !Equals(left, right);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (obj.GetType() != typeof (Event))
                return false;

            return Equals((Event) obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static Event operator +(Event a, Event b)
        {
            return UnionEvent.Get(a, b);
        }

        public static Event operator *(Event a, Event b)
        {
            return IntersectionEvent.Get(a, b);
        }

        public static Event operator |(Event a, Event b)
        {
            return ConditionalEvent.Get(a, b);
        }

        public static Event operator !(Event a)
        {
            return ComplementEvent.Get(a);
        }

        public static Event operator -(Event a, Event b)
        {
            return a*!b;
        }

        #endregion
    }
}
