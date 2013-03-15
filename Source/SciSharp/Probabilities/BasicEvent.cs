namespace SciSharp.Probabilities
{
    public class BasicEvent : Event
    {
        private static string LastName = "";

        public BasicEvent(string name = null)
        {
            Name = name;

            if (name == null)
                Name = GetNewName();
        }

        public string Name { get; set; }

        private static string GetNewName()
        {
            LastName = string.IsNullOrEmpty(LastName) ? "A" : ((char) (LastName[0] + 1)).ToString();
            return LastName;
        }

        public override bool IsExclusive(Event other)
        {
            return base.IsExclusive(other) || (!(other is BasicEvent) && other.IsExclusive(this));
        }

        public override bool IsIndependent(Event other)
        {
            return base.IsIndependent(other) || (!(other is BasicEvent) && other.IsIndependent(this));
        }

        public override bool IsSubset(Event other)
        {
            return base.IsSubset(other) || (!(other is BasicEvent) && other.IsSuperset(this));
        }

        public override bool IsSuperset(Event other)
        {
            return base.IsSuperset(other) || (!(other is BasicEvent) && other.IsSubset(this));
        }

        protected internal override bool TryCalculateProbability(ProbabilityFunction p, out double result, out Formulae formulae)
        {
            formulae = null;

            if (p.TryGetProbability(this, out result))
                return true;

            foreach (var partition in p.Partitions)
            {
                double total = 0;
                bool done = true;

                foreach (Event b in partition)
                {
                    if (p.TryGetProbability(this*b, out result))
                        total += result;
                    else
                    {
                        done = false;
                        break;
                    }
                }

                if (done)
                {
                    result = total;
                    return true;
                }
            }

            return false;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
