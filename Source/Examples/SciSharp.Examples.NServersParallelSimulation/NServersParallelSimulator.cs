using System.Collections.Generic;

using SciSharp.Collections;
using SciSharp.Probabilities;
using SciSharp.Simulation.DiscreteEvents;


namespace SciSharp.Examples.NServersParallelSimulation
{
    public class NServersParallelSimulator : DiscreteEventsSimulator,
                                             IEventProcessor<ClientArriveEvent>,
                                             IEventProcessor<ServerFinishEvent>
    {
        private readonly RandomVariable clientArrive;
        private readonly double closeTime;
        private readonly BinaryHeap<int> freeServers;
        private readonly int[] serverBusy;
        private readonly RandomVariable serverDelay;

        private int clientsInQueue;
        private int nextClient = 1;

        public NServersParallelSimulator(int servers, RandomVariable clientArrive, RandomVariable serverDelay, double closeTime)
        {
            this.clientArrive = clientArrive;
            this.serverDelay = serverDelay;
            this.closeTime = closeTime;

            Servers = servers;
            serverBusy = new int[servers];

            freeServers = new BinaryHeap<int>(servers);

            for (int i = 0; i < servers; i++)
                freeServers.Add(i);

            Enqueue(new ClientArriveEvent(clientArrive));
        }

        public int ClientsInQueue
        {
            get { return clientsInQueue; }
        }

        public double CloseTime
        {
            get { return closeTime; }
        }

        public int Servers { get; private set; }

        public int Clients
        {
            get { return nextClient; }
        }

        #region IEventProcessor<ClientArriveEvent> Members

        public IEnumerable<DiscreteEvent> Process(ClientArriveEvent ev)
        {
            // Si el evento está después del tiempo final, se obvia
            if (ev.TimeStamp > closeTime)
                yield break;

            // Si hay un servidor libre, lo ponemos en ese servidor
            if (freeServers.Count > 0)
            {
                // Primer servidor libre
                int server = freeServers.Extract();
                serverBusy[server] = nextClient++;

                // Generar la salida de este cliente
                yield return new ServerFinishEvent(CurrentTime + serverDelay, server);
            }
            else
            {
                // Encolar este cliente
                clientsInQueue++;
            }

            // Generar el siguiente cliente
            yield return new ClientArriveEvent(CurrentTime + clientArrive);
        }

        #endregion

        #region IEventProcessor<ServerFinishEvent> Members

        public IEnumerable<DiscreteEvent> Process(ServerFinishEvent ev)
        {
            // Si hay clientes en cola, cogemos uno
            if (clientsInQueue > 0)
            {
                clientsInQueue--;
                serverBusy[ev.Server] = nextClient++;

                // Generar la salida de este mismo servidor
                yield return new ServerFinishEvent(CurrentTime + serverDelay, ev.Server);
            }
            else
            {
                // De lo contrario, encolar este servidor
                freeServers.Add(ev.Server);
                serverBusy[ev.Server] = 0;
            }
        }

        #endregion

        public int ServerBusy(int server)
        {
            return serverBusy[server];
        }
    }
}
