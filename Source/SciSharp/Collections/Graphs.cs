using System;
using System.Collections.Generic;


namespace SciSharp.Collections
{
    public static class Graphs
    {
        public static IGraph<T, WeightedEdge<T>> AsWeighted<T>(this IGraph<T> graph)
        {
            return AsWeighted(graph, x => 1d);
        }

        public static IGraph<T, WeightedEdge<T>> AsWeighted<T>(this IGraph<T> graph, Func<Edge<T>, double> edgeWeight)
        {
            if (graph == null)
                throw new ArgumentNullException("graph");

            if (edgeWeight == null)
                throw new ArgumentNullException("edgeWeight");

            return new WeightedGraph<T>(graph, edgeWeight);
        }

        #region Nested type: WeightedGraph

        public class WeightedGraph<T> : IGraph<T, WeightedEdge<T>>
        {
            private readonly Func<Edge<T>, double> edgeWeight;
            private readonly IGraph<T> graph;

            public WeightedGraph(IGraph<T> graph, Func<Edge<T>, double> edgeWeight)
            {
                this.graph = graph;
                this.edgeWeight = edgeWeight;
            }

            #region IGraph<T,WeightedEdge<T>> Members

            public IEnumerable<T> AdyacentOf(T node)
            {
                return graph.AdyacentOf(node);
            }

            public WeightedEdge<T> this[T u, T v]
            {
                get { return new WeightedEdge<T>(u, v, edgeWeight(graph[u, v])); }
                set { throw new InvalidOperationException("Cannot set the weight of a dynamically created weighted graph."); }
            }

            public int Degree(T node)
            {
                return graph.Degree(node);
            }

            #endregion
        }

        #endregion
    }
}
