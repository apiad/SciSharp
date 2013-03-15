using System;


namespace SciSharp.Simulation.DiscreteEvents
{
    public class DiscreteEventArgs : EventArgs
    {
        private readonly DiscreteEvent currentEvent;
        private readonly double currentTime;
        private readonly int eventsInQueue;

        public DiscreteEventArgs(double currentTime, DiscreteEvent currentEvent, int eventsInQueue)
        {
            this.currentTime = currentTime;
            this.currentEvent = currentEvent;
            this.eventsInQueue = eventsInQueue;
        }

        public double CurrentTime
        {
            get { return currentTime; }
        }

        public DiscreteEvent CurrentEvent
        {
            get { return currentEvent; }
        }

        public int EventsInQueue
        {
            get { return eventsInQueue; }
        }
    }
}
