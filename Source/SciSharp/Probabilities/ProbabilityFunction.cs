using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;


namespace SciSharp.Probabilities
{
    public class ProbabilityFunction
    {
        private readonly HashSet<Event> calculating = new HashSet<Event>();

        private readonly List<Event[]> partitions = new List<Event[]>();
        private readonly Dictionary<Event, double> probability;

        public ProbabilityFunction()
        {
            probability = new Dictionary<Event, double>();
        }

        public double this[Event x]
        {
            get
            {
                double p;
                Formulae f;

                if (!TryGetProbability(x, out p, out f))
                    throw new InvalidOperationException("Cannot calculate probability of event " + x);

                return this[x] = p;
            }
            set { probability[x] = value; }
        }

        public IEnumerable<Event[]> Partitions
        {
            get { return partitions; }
        }

        public bool TryGetProbability(Event x, out double p)
        {
            Formulae formulae;
            return TryGetProbability(x, out p, out formulae);
        }

        public bool TryGetProbability(Event x, out double p, out Formulae formulae)
        {
            p = 0;
            formulae = new Formulae(x, p);

            if (calculating.Contains(x))
                return false;

            if (probability.TryGetValue(x, out p))
            {
                formulae.Probability = p;
                return true;
            }

            calculating.Add(x);
            bool ok = x.TryCalculateProbability(this, out p, out formulae);
            calculating.Remove(x);

            return ok;
        }

        public Event NewEvent(double p, string name = null)
        {
            var ev = new BasicEvent(name);
            this[ev] = p;

            MakePartition(ev, !ev);

            return ev;
        }

        public void MakePartition(params Event[] events)
        {
            partitions.Add(events);

            for (int i = 0; i < events.Length - 1; i++)
                for (int j = i + 1; j < events.Length; j++)
                    events[i].MakeExclusive(events[j]);
        }
    }

    public delegate double ProbabilityDistribution(int x);

    public delegate double ProbabilityDensity(double x);

    public class Formulae : IEnumerable<Formulae>
    {
        public Formulae(Event root, double probability)
        {
            Root = root;
            Probability = probability;
            Children = new List<Formulae>();
        }

        public Event Root { get; private set; }
        public double Probability { get; set; }
        public List<Formulae> Children { get; private set; }

        #region IEnumerable<Formulae> Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<Formulae> GetEnumerator()
        {
            return Children.GetEnumerator();
        }

        #endregion

        public void Add(Formulae child)
        {
            Children.Add(child);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            ToString(0, sb);
            return sb.ToString();
        }

        private void ToString(int depth, StringBuilder sb)
        {
            for (int i = 0; i < depth; i++)
                sb.Append(' ');

            sb.AppendFormat("{1} = {0}\n", Probability, Root);

            foreach (Formulae formulae in Children)
                formulae.ToString(depth + 1, sb);
        }
    }
}
