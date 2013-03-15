using System;
using System.Collections.Generic;
using System.Reflection;

using SciSharp.Collections;


namespace SciSharp.Simulation.DiscreteEvents
{
    public abstract class DiscreteEventsSimulator
    {
        private readonly BinaryHeap<DiscreteEvent> queue;

        private double currentTime;
        private double lastTime;
        private double nextTime;

        private bool processing;

        protected DiscreteEventsSimulator()
            : this(1024) {}

        protected DiscreteEventsSimulator(int size)
        {
            queue = new BinaryHeap<DiscreteEvent>(size);
        }

        public double LastTime
        {
            get { return lastTime; }
        }

        public double CurrentTime
        {
            get { return currentTime; }
        }

        public double NextTime
        {
            get
            {
                if (processing)
                    throw new InvalidOperationException("This value cannot be used while processing an event.");

                return nextTime;
            }
        }

        protected void Enqueue(DiscreteEvent ev)
        {
            ev.creationTime = CurrentTime;

            OnEventAdding(ev);
            queue.Add(ev);
            OnEventAdded(ev);
        }

        private IEnumerable<DiscreteEvent> Process(DiscreteEvent nextEvent)
        {
            #region Warning: Reflection magic. Just for heavy stomachs...

            Type eventType = nextEvent.GetType();
            Type thisInterface = typeof (IEventProcessor<>).MakeGenericType(eventType);

            return (IEnumerable<DiscreteEvent>) thisInterface.InvokeMember("Process", BindingFlags.InvokeMethod, null,
                                                                           this, new object[] {nextEvent});

            #endregion
        }

        public bool NextEvent()
        {
            if (queue.Empty)
                return false;

            DiscreteEvent next = queue.Extract();
            lastTime = currentTime;
            currentTime = next.TimeStamp;

            processing = true;

            OnEventProcessing(next);

            IEnumerable<DiscreteEvent> events = Process(next);

            OnEventProcessed(next);

            foreach (DiscreteEvent ev in events)
                Enqueue(ev);

            processing = false;

            if (!queue.Empty)
                nextTime = queue.Min.TimeStamp;

            return true;
        }

        public void Run(double time)
        {
            double end = currentTime + time;
            RunUntil(end);
        }

        private void RunUntil(double time)
        {
            while (currentTime < time)
                if (!NextEvent())
                    return;
        }

        public void Run()
        {
            RunUntil(double.PositiveInfinity);
        }

        public event EventHandler<DiscreteEventArgs> EventAdded;

        public event EventHandler<DiscreteEventArgs> EventAdding;

        public event EventHandler<DiscreteEventArgs> EventProcessed;

        public event EventHandler<DiscreteEventArgs> EventProcessing;

        protected virtual void OnEventAdded(DiscreteEvent e)
        {
            EventHandler<DiscreteEventArgs> handler = EventAdded;

            if (handler != null)
                handler(this, new DiscreteEventArgs(CurrentTime, e, queue.Count));
        }

        protected virtual void OnEventAdding(DiscreteEvent e)
        {
            EventHandler<DiscreteEventArgs> handler = EventAdding;

            if (handler != null)
                handler(this, new DiscreteEventArgs(CurrentTime, e, queue.Count));
        }

        protected virtual void OnEventProcessed(DiscreteEvent e)
        {
            EventHandler<DiscreteEventArgs> handler = EventProcessed;

            if (handler != null)
                handler(this, new DiscreteEventArgs(CurrentTime, e, queue.Count));
        }

        protected virtual void OnEventProcessing(DiscreteEvent e)
        {
            EventHandler<DiscreteEventArgs> handler = EventProcessing;

            if (handler != null)
                handler(this, new DiscreteEventArgs(CurrentTime, e, queue.Count));
        }
    }
}
