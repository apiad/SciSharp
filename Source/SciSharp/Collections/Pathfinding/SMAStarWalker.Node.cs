using System.Collections.Generic;
using System.Linq;


namespace SciSharp.Collections.Pathfinding
{
    public partial class SmaStarWalker<TNode, TEdge>
    {
        #region Nested type: Node

        public class Node
        {
            public static int LastAge;
            public static int LastId;
            public readonly double Cost;
            public readonly double[] Costs;
            public readonly int Depth;
            public readonly IEnumerable<TNode> Enumerable;
            public readonly bool[] Forgotten;
            public readonly bool[] Generated;

            public readonly int Id;
            public readonly Node Parent;
            public readonly int ParentIndex;
            public readonly bool[] Skipped;
            public readonly TNode State;
            public readonly int SuccesorsCount;

            private readonly IGraph<TNode, TEdge> graph;
            public int Age;
            public double FCost;
            public IEnumerator<TNode> Generator;
            private bool hasNext;
            private int nextIndex;

            public Node(TNode state, Node parent, double fCost, int depth, double cost, int parentIndex, IGraph<TNode, TEdge> graph)
            {
                this.graph = graph;
                IEnumerable<TNode> enumerable = graph.AdyacentOf(state);

                Parent = parent;
                State = state;
                FCost = fCost;
                Depth = depth;
                ParentIndex = parentIndex;
                Generator = enumerable.GetEnumerator();
                Enumerable = enumerable;
                SuccesorsCount = graph.Degree(state);
                Generated = new bool[SuccesorsCount];
                Forgotten = new bool[SuccesorsCount];
                Skipped = new bool[SuccesorsCount];
                Costs = Arrays.Fill(double.PositiveInfinity, SuccesorsCount);
                Age = LastAge++;
                Id = LastId++;

                Cost = cost;
                hasNext = Generator.MoveNext();
            }

            public int SuccesorsInMemory
            {
                get
                {
                    int count = 0;

                    for (int i = 0; i < SuccesorsCount; i++)
                        if (Generated[i] || Skipped[i])
                            count++;

                    return count;
                }
            }

            public int SuccesorsForgotten
            {
                get
                {
                    int count = 0;

                    for (int i = 0; i < SuccesorsCount; i++)
                        if (Forgotten[i]) count++;

                    return count;
                }
            }

            public int SuccesorsSkipped
            {
                get
                {
                    int count = 0;

                    for (int i = 0; i < SuccesorsCount; i++)
                        if (Skipped[i]) count++;

                    return count;
                }
            }

            public bool IsRoot
            {
                get { return Parent == null; }
            }

            public bool Completed
            {
                get
                {
                    for (int i = 0; i < SuccesorsCount; i++)
                        if (!Generated[i] && !Forgotten[i] && !Skipped[i])
                            return false;

                    return true;
                }
            }

            public bool AllInMemory
            {
                // Cuando todos los hijos estén en la memoria, este nodo
                // debe ser cerrado, pero solo si no es hoja.
                // Esto puede pasar si todos los hijos de un nodo
                // son skipped, lo que quiere decir que en la práctica
                // este nodo siempre será una hoja. En ese caso
                // hay que dejarlo abierto para ser eliminado en el
                // momento correcto (notar que un nodo así tendrá
                // siempre FCost = double.PositiveInfinity). 
                get { return SuccesorsInMemory == SuccesorsCount && !IsLeaf; }
            }

            public bool IsLeaf
            {
                get
                {
                    for (int i = 0; i < SuccesorsCount; i++)
                        if (Generated[i]) return false;

                    return true;
                }
            }

            public void Own(Node node)
            {
                if (Generated[node.ParentIndex])
                    throw new NowWhatException();

                Generated[node.ParentIndex] = true;
                Forgotten[node.ParentIndex] = false;
                Skipped[node.ParentIndex] = false;
                Costs[node.ParentIndex] = node.FCost;
            }

            public void Forget(Node node)
            {
                if (Forgotten[node.ParentIndex])
                    throw new NowWhatException();

                Generated[node.ParentIndex] = false;
                Skipped[node.ParentIndex] = false;
                Forgotten[node.ParentIndex] = true;
                Costs[node.ParentIndex] = node.FCost;
            }

            public void Skip(Node node)
            {
                if (Skipped[node.ParentIndex])
                    throw new NowWhatException();

                Generated[node.ParentIndex] = false;
                Forgotten[node.ParentIndex] = false;
                Skipped[node.ParentIndex] = true;
                Costs[node.ParentIndex] = double.PositiveInfinity;
            }

            public Node Next()
            {
                // Saltarse todos los generados
                while (hasNext && (Generated[nextIndex] || Skipped[nextIndex]))
                {
                    nextIndex++;
                    hasNext = Generator.MoveNext();
                }

                // No hay mas sucesores, reiniciar el generador
                if (!hasNext)
                {
                    Generator = Enumerable.GetEnumerator();
                    hasNext = Generator.MoveNext();
                    nextIndex = 0;

                    return null;
                }

                // Nos queda algun nodo, lo generamos
                var next = new Node(Generator.Current, this, double.PositiveInfinity, Depth + 1, Cost + graph[State, Generator.Current].Weight, nextIndex++, graph);
                hasNext = Generator.MoveNext();

                // !! Este nodo tiene que ser marcado como generado en el algoritmo

                return next;
            }

            public bool UpdateFCost()
            {
                bool change = false;

                double min = Costs.Min();

                if (min != FCost)
                {
                    FCost = min;
                    change = true;
                }

                if (Parent != null)
                    Parent.Costs[ParentIndex] = FCost;

                return change;
            }

            /// <summary>
            /// Returns a string that represents the current object.
            /// </summary>
            /// <returns>
            /// A string that represents the current object.
            /// </returns>
            /// <filterpriority>2</filterpriority>
            public override string ToString()
            {
                return string.Format("Id: {4}, FCost: {0}, Depth: {1}, Age: {2}, IsLeaf: {3}", FCost, Depth, Age, IsLeaf, Id);
            }

            public bool Equals(Node other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return Equals(other.State, State);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != typeof (Node)) return false;
                return Equals((Node) obj);
            }

            public override int GetHashCode()
            {
                return State.GetHashCode();
            }
        }

        #endregion
    }
}
