using System;


namespace SciSharp.Simulation.DiscreteEvents
{
    public abstract class DiscreteEvent : IComparable<DiscreteEvent>
    {
        private readonly double timeStamp;

        internal double creationTime;

        protected DiscreteEvent(double timeStamp)
        {
            this.timeStamp = timeStamp;
        }

        public double CreationTime
        {
            get { return creationTime; }
        }

        public double TimeStamp
        {
            get { return timeStamp; }
        }

        #region IComparable<DiscreteEvent> Members

        public int CompareTo(DiscreteEvent other)
        {
            return TimeStamp.CompareTo(other.TimeStamp);
        }

        #endregion

        public override string ToString()
        {
            string name = GetType().Name;

            if (name.ToLower().EndsWith("event"))
                name = name.Substring(0, name.Length - 5);

            return string.Format("Time: {0} => Event: {1}", timeStamp, name);
        }
    }
}
