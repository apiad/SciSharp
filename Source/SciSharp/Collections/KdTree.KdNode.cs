using System;
using System.Collections.Generic;


namespace SciSharp.Collections
{
    public partial class KdTree
    {
        #region Nested type: KdNode

        public class KdNode
        {
            public readonly int Count;
            public readonly int Depth;
            private readonly int dimension;
            private readonly KdNode left;
            private readonly double partition;
            private readonly KdNode right;
            private readonly Vector vector;

            public KdNode(double partition, int dimension, KdNode left, KdNode right, Vector vector)
            {
                this.partition = partition;
                this.dimension = dimension;
                this.left = left;
                this.right = right;
                this.vector = vector;

                int leftCount = left != null ? left.Count : 0;
                int rightCount = right != null ? right.Count : 0;

                Count = leftCount + rightCount + 1;

                int leftDepht = left != null ? left.Depth : 0;
                int rightDepht = right != null ? right.Depth : 0;

                Depth = Math.Max(leftDepht, rightDepht) + 1;
            }

            /// <summary>
            /// Devuelve si un nodo es una hoja
            /// </summary>
            public bool IsLeaf
            {
                get { return left == null && right == null; }
            }

            /// <summary>
            /// Encuentra los k vectores más cercanos.
            /// </summary>
            /// <param name="count">cantidad de vectores cercanos</param>
            /// <param name="vector">Vector al que se le buscanlos cercanos</param>
            /// <returns></returns>
            public IEnumerable<Vector> Closest(int count, Vector vector, float maxDist = float.MaxValue)
            {
                return Closest(count, vector, cmp => new BinaryHeap<Vector>(cmp), maxDist);
            }

            /// <summary>
            /// Encuentra los k vectores más cercanos.
            /// </summary>
            /// <param name="count">cantidad de vectores cercanos</param>
            /// <param name="p">Vector al que se le buscanlos cercanos</param>
            /// <param name="factory">un constructor de heaps basado en una funcion de comparacion particular</param>
            /// <returns></returns>
            private IEnumerable<Vector> Closest(int count, Vector p, Func<Comparison<Vector>, BinaryHeap<Vector>> factory, float maxDist = float.MaxValue)
            {
                BinaryHeap<Vector> heap = factory((x, y) => (p - x).LengthSqr.CompareTo((p - y).LengthSqr));
                FindClosest(count, p, heap, maxDist*maxDist);
                return heap;
            }

            /// <summary>
            /// Encuentra los k vectores más cercanos.
            /// </summary>
            /// <param name="count">cantidad de vectores cercanos a devolver</param>
            /// <param name="p">Vector al que se le buscanlos cercanos</param>
            /// <param name="closests">lista de cercanos</param>
            /// <param name="maxDist">The maximum distance to search.</param>
            private void FindClosest(int count, Vector p, BinaryHeap<Vector> closests, float maxDist = float.MaxValue)
            {
                //Si la cantidad de cercanos es menor que el count requerido, lo agrego sin comparar
                if (closests.Count < count)
                {
                    if (vector.DistanceSqrTo(p) < maxDist)
                        closests.Add(vector);
                }

                    //Me quedo con el Vector si es más cercano que alguno de los que tenga
                else
                {
                    Vector max = closests.Min;
                    double d = vector.DistanceSqrTo(p);

                    // Sustituir este Vector por el mas cercano
                    if (d < (max - p).LengthSqr && d < maxDist)
                    {
                        closests.Extract();
                        closests.Add(vector);
                    }
                }

                if (IsLeaf)
                    return;

                //Calcular la distancia al plano por donde pico
                double dist = p[dimension] - partition;

                KdNode first, second;

                //Encuentro en cual de los hijos busco primero (dimensión con plano)
                if (p[dimension] < partition)
                {
                    first = left;
                    second = right;
                }
                else
                {
                    first = right;
                    second = left;
                }

                // Siempre busco en el primero
                if (first != null)
                    first.FindClosest(count, p, closests, maxDist);

                // Busco en el segundo solo si tengo vectores mas alejados que el plano de corte
                if (second != null && (closests.Count == 0 || (closests.Min - p).LengthSqr > dist*dist) && dist*dist < maxDist)
                    second.FindClosest(count, p, closests, maxDist);
            }
        }

        #endregion
    }
}
