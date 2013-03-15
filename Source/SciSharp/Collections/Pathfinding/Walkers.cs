using System;
using System.Collections.Generic;


namespace SciSharp.Collections.Pathfinding
{
    public static class Walkers
    {
        public static Walk<TNode, TEdge> Walk<TNode, TEdge>(this IGraphWalker<TNode, TEdge> walker, IGraph<TNode, TEdge> graph, TNode start)
            where TEdge : Edge<TNode>
        {
            return Walk(walker, graph, start, x => false);
        }

        public static Walk<TNode, TEdge> Walk<TNode, TEdge>(this IGraphWalker<TNode, TEdge> walker, IGraph<TNode, TEdge> graph, TNode start, TNode end)
            where TEdge : Edge<TNode>
        {
            return Walk(walker, graph, start, x => x.Equals(end));
        }

        public static Walk<TNode, TEdge> Walk<TNode, TEdge>(this IGraphWalker<TNode, TEdge> walker, IGraph<TNode, TEdge> graph, TNode start, Predicate<TNode> goal)
            where TEdge : Edge<TNode>
        {
            bool found;
            TNode end;
            return Walk(walker, graph, start, goal, out found, out end);
        }

        public static Walk<TNode, TEdge> Walk<TNode, TEdge>(this IGraphWalker<TNode, TEdge> walker, IGraph<TNode, TEdge> graph, TNode start, Predicate<TNode> goal, out bool found, out TNode end)
            where TEdge : Edge<TNode>
        {
            var crossings = new Dictionary<TNode, TEdge>();
            found = false;
            end = default(TNode);

            foreach (TEdge edge in walker.Walk(graph, start))
            {
                crossings[edge.Right] = edge;

                if (goal(edge.Right))
                {
                    end = edge.Right;
                    found = true;
                    break;
                }
            }

            return new Walk<TNode, TEdge>(crossings);
        }
    }
}
