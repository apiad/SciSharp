using System;
using System.Collections.Generic;
using System.Threading;


namespace SciSharp.Collections.Pathfinding
{
    public partial class SmaStarWalker<TNode, TEdge>
        where TEdge : WeightedEdge<TNode>
    {
        protected readonly Func<TNode, double> Heuristic;

        public readonly bool Optimal;
        protected readonly int Size;
        private readonly CancellationTokenSource cancel;

        public SmaStarWalker(Func<TNode, double> heuristic, int size, bool optimal = false, CancellationTokenSource cancel = null)
        {
            Heuristic = heuristic;
            Size = size;
            Optimal = optimal;
            this.cancel = cancel;
        }

        public Walk<TNode, TEdge> Walk(IGraph<TNode, TEdge> graph, TNode root, TNode end)
        {
            Node.LastId = 0;
            Node.LastAge = 0;

            var rootNode = new Node(root, null, Heuristic(root), 0, 0d, -1, graph);
            var fringe = new Fringe();
            fringe.Open(rootNode);

            var walk = new Dictionary<TNode, TEdge>();

            while (fringe.Count > 0)
            {
                cancel.Token.ThrowIfCancellationRequested();

                // Obtener el mejor nodo para expandir
                Node best = fringe.Best;

                // Chequear condición de optimalidad
                if (best.State.Equals(end) && (!Optimal || rootNode.FCost == best.Cost))
                    break;

                if (Node.LastId%10000 == 0)
                    Console.WriteLine("SMA* reporting: {0} nodes.", Node.LastId);

                // Obtener el siguiente sucesor
                Node next = best.Next();

                // Saltarse todos los repetidos
                while (next != null && fringe.Contains(next))
                {
                    //Console.WriteLine("Skipped: {0}", next.ParentIndex);

                    // Marcar este nodo como saltado
                    best.Skip(next);

                    // Actualizar la condicion de hoja
                    fringe.UpdateLeafStatus(best);

                    // Siguiente nodo
                    next = best.Next();
                }

                // Actualizar el costo del sucesor
                if (next != null)
                {
                    //Console.WriteLine("Opened: {0} ({1})", next.ParentIndex, next);

                    // Actualizar el costo real del camino
                    walk[next.State] = graph[best.State, next.State];

                    // Actualizar el estimado manteniendo la monotonicidad
                    if (next.Depth < Size)
                        next.FCost = Math.Max(best.FCost, next.Cost + Heuristic(next.State));

                    // Este nodo va a la cola, actualizar en el padre
                    best.Own(next);

                    // Ahora best ya no es una hoja
                    fringe.UpdateLeafStatus(best);

                    // Insertar en la cola
                    fringe.Open(next);
                }

                // Si el padre esta completo actualizar los costos hasta arriba
                if (best.Completed)
                    for (Node parent = best; parent != null && parent.Completed; parent = parent.Parent)
                    {
                        // Si estaba en la frontera, hay que sacarlo antes de actualizar
                        bool wasOpen = fringe.RemoveOpen(parent);

                        // Saber si es necesario seguir actualizando
                        bool update = parent.UpdateFCost();

                        // Volver a poner en la frontera
                        if (wasOpen)
                            fringe.Open(parent);

                        if (!update)
                            break;
                    }

                // Si todos los sucesores estan en la cola, quitar a best de la cola
                if (best.AllInMemory)
                {
                    fringe.Close(best);
                    //Console.WriteLine("Closed: {0}", best);
                }

                if (next == null && !best.AllInMemory && !best.Completed)
                    throw new NowWhatException();

                // Si la memoria esta llena, hacer la magia
                if (fringe.Size > Size)
                {
                    // Obtener el nodo a borrar
                    Node worst = fringe.RemoveWorst();

                    //Console.WriteLine("Dropped: {0}", worst);

                    // Descruzar la arista
                    walk.Remove(worst.State);

                    // Actualizar a su padre
                    Node parent = worst.Parent;

                    // Olvidar al nodo
                    parent.Forget(worst);

                    // Insertar al padre en la frontera si es necesario
                    // y chequear su condicion de hoja
                    if (fringe.IsClosed(parent))
                        fringe.ReOpen(parent);
                    else
                        fringe.UpdateLeafStatus(parent);
                }
            }

            return new Walk<TNode, TEdge>(walk);
        }
    }
}
