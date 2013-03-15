using SciSharp.Simulation.DiscreteEvents;


namespace SciSharp.Examples.NServersParallelSimulation
{
    public class ClientArriveEvent : DiscreteEvent
    {
        public ClientArriveEvent(double timeStamp)
            : base(timeStamp) {}
    }
}
