using SciSharp.Simulation.DiscreteEvents;


namespace SciSharp.Examples.NServersParallelSimulation
{
    public class ServerFinishEvent : DiscreteEvent
    {
        private readonly int server;

        public ServerFinishEvent(double timeStamp, int server)
            : base(timeStamp)
        {
            this.server = server;
        }

        public int Server
        {
            get { return server; }
        }
    }
}
