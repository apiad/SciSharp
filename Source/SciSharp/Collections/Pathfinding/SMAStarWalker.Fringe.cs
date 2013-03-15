using System;
using System.Collections.Generic;
using System.Linq;


namespace SciSharp.Collections.Pathfinding
{
    public partial class SmaStarWalker<TNode, TEdge>
    {
        #region Nested type: Fringe

        public class Fringe
        {
            // Esta clase se usa para almacenar el fringe de SMA*.
            // El problema radica en que para expandir se debe seleccionar
            // el nodo con menor f-cost y para borrar el nodo hoja con
            // mayor f-cots. No existe una forma de incluir la condición
            // de ser hoja en un comparador y que quede antisimétrico,
            // por lo que se deben tener dos condiciones diferentes.
            // Además como el criterio de pertenencia al fringe incluye
            // un hash, hay que mantener varias estructuras sincronizadas.

            private readonly HashSet<Node> closed;
            private readonly AvlTree<Node> fringe;
            private readonly AvlTree<Node> leaves;
            private readonly HashSet<Node> open;

            public Fringe()
            {
                fringe = new AvlTree<Node>(CmpBest);
                leaves = new AvlTree<Node>(CmpWorst);

                open = new HashSet<Node>();
                closed = new HashSet<Node>();
            }

            public Node Best
            {
                get { return fringe.Min; }
            }

            public int Count
            {
                get { return fringe.Count; }
            }

            public int Size
            {
                get { return open.Count + closed.Count; }
            }

            public ICollection<Node> Leaves
            {
                get { return leaves; }
            }

            /// <summary>
            /// Compara dos nodos por el criterio de
            /// el de menor costo, mas profundo, mas nuevo.
            /// </summary>
            private int CmpBest(Node x, Node y)
            {
                if (x.FCost != y.FCost)
                    return x.FCost.CompareTo(y.FCost);

                if (x.Depth != y.Depth)
                    return -x.Depth.CompareTo(y.Depth);

                return -x.Age.CompareTo(y.Age);
            }

            /// <summary>
            /// Compara dos nodos por el criterio de
            /// el de mayor costo, menor profundidad, mas viejo
            /// </summary>
            private int CmpWorst(Node x, Node y)
            {
                if (x.FCost != y.FCost)
                    return -x.FCost.CompareTo(y.FCost);

                //if (x.Depth != y.Depth)
                //    return x.Depth.CompareTo(y.Depth);

                return x.Age.CompareTo(y.Age);
            }

            public bool IsOpen(Node node)
            {
                return open.Contains(node);
            }

            public bool IsClosed(Node node)
            {
                return closed.Contains(node);
            }

            public bool Contains(Node node)
            {
                return open.Contains(node) || closed.Contains(node);
            }

            public void Open(Node node)
            {
                if (closed.Remove(node))
                    throw new InvalidOperationException("The node was closed. Call ReOpen instead.");

                if (!open.Add(node))
                    throw new InvalidOperationException("The node was already open.");

                fringe.Add(node);

                if (node.IsLeaf)
                    leaves.Add(node);
            }

            public void UpdateLeafStatus(Node node)
            {
                if (node.IsLeaf && !leaves.Contains(node))
                    leaves.Add(node);
                else if (!node.IsLeaf)
                    leaves.Remove(node);
            }

            public bool RemoveOpen(Node node)
            {
                bool isOpen = open.Remove(node);

                if (isOpen)
                {
                    fringe.Remove(node);
                    leaves.Remove(node);
                }

                return isOpen;
            }

            public void Close(Node node)
            {
                if (!open.Remove(node))
                    throw new InvalidOperationException("The node was not open.");

                fringe.Remove(node);
                leaves.Remove(node);
                closed.Add(node);
            }

            public Node RemoveWorst()
            {
                Node worst = leaves.RemoveMin();

                if (closed.Contains(worst))
                    throw new NowWhatException("The worst node is closed!");

                fringe.Remove(worst);
                open.Remove(worst);

                return worst;
            }

            public void ReOpen(Node parent)
            {
                if (!closed.Remove(parent))
                    throw new InvalidOperationException("The node was not closed.");

                parent.Age = Node.LastAge++;

                Open(parent);
            }

            public void Check()
            {
                if (fringe.Count(n => n.IsLeaf) != leaves.Count)
                    throw new NowWhatException("Inconsistent # of leaves.");
            }
        }

        #endregion
    }
}
