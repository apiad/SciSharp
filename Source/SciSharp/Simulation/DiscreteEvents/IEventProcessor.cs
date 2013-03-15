using System.Collections.Generic;


namespace SciSharp.Simulation.DiscreteEvents
{
    public interface IEventProcessor<in T>
        where T : DiscreteEvent
    {
        IEnumerable<DiscreteEvent> Process(T ev);
    }
}
