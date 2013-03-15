using System;
using System.Collections.Generic;
using System.Threading;


namespace SciSharp.Collections.Pathfinding
{
    public static class Search
    {
        public static Walk<TNode, TEdge> Dfs<TNode, TEdge>(this IGraph<TNode, TEdge> graph, TNode start, TNode goal)
            where TEdge : Edge<TNode>
        {
            return Dfs(graph, start, x => x.Equals(goal));
        }

        public static Walk<TNode, TEdge> Dfs<TNode, TEdge>(this IGraph<TNode, TEdge> graph, TNode start, Predicate<TNode> goal)
            where TEdge : Edge<TNode>
        {
            return new DepthFirstWalker<TNode, TEdge>().Walk(graph, start, goal);
        }

        public static Walk<TNode, TEdge> Dfs<TNode, TEdge>(this IGraph<TNode, TEdge> graph, TNode start, Predicate<TNode> goal, out bool found, out TNode end)
            where TEdge : Edge<TNode>
        {
            return new DepthFirstWalker<TNode, TEdge>().Walk(graph, start, goal, out found, out end);
        }

        public static Walk<TNode, TEdge> Bfs<TNode, TEdge>(this IGraph<TNode, TEdge> graph, TNode start, TNode goal)
            where TEdge : Edge<TNode>
        {
            return Bfs(graph, start, x => x.Equals(goal));
        }

        public static Walk<TNode, TEdge> Bfs<TNode, TEdge>(this IGraph<TNode, TEdge> graph, TNode start, Predicate<TNode> goal)
            where TEdge : Edge<TNode>
        {
            return new BreadthFirstWalker<TNode, TEdge>().Walk(graph, start, goal);
        }

        public static Walk<TNode, TEdge> Bfs<TNode, TEdge>(this IGraph<TNode, TEdge> graph, TNode start, Predicate<TNode> goal, out bool found, out TNode end)
            where TEdge : Edge<TNode>
        {
            return new BreadthFirstWalker<TNode, TEdge>().Walk(graph, start, goal, out found, out end);
        }

        public static Walk<TNode, TEdge> SMAStar<TNode, TEdge>(this IGraph<TNode, TEdge> graph, TNode start, TNode goal, Func<TNode, double> heuristic, int size, CancellationTokenSource cancel, bool optimal)
            where TEdge : WeightedEdge<TNode>
        {
            return new SmaStarWalker<TNode, TEdge>(heuristic, size, optimal, cancel).Walk(graph, start, goal);
        }

        public static Walk<TNode, TEdge> AStar<TNode, TEdge>(this IGraph<TNode, TEdge> graph, TNode start, TNode goal, Func<TNode, double> heuristic)
            where TEdge : WeightedEdge<TNode>
        {
            return AStar(graph, start, x => x.Equals(goal), heuristic);
        }

        public static Walk<TNode, TEdge> AStar<TNode, TEdge>(this IGraph<TNode, TEdge> graph, TNode start, Predicate<TNode> goal, Func<TNode, double> heuristic)
            where TEdge : WeightedEdge<TNode>
        {
            return new AStarWalker<TNode, TEdge>(heuristic).Walk(graph, start, goal);
        }

        public static Walk<TNode, TEdge> AStar<TNode, TEdge>(this IGraph<TNode, TEdge> graph, TNode start, Predicate<TNode> goal, Func<TNode, double> heuristic, out bool found, out TNode end)
            where TEdge : WeightedEdge<TNode>
        {
            return new AStarWalker<TNode, TEdge>(heuristic).Walk(graph, start, goal, out found, out end);
        }

        public static IEnumerable<KeyValuePair<TNode, int>> Distance<TNode, TEdge>(this IGraph<TNode, TEdge> graph, TNode root)
            where TEdge : Edge<TNode>
        {
            var distance = new Dictionary<TNode, int>();
            var walker = new BreadthFirstWalker<TNode, TEdge>();

            distance[root] = 0;

            yield return new KeyValuePair<TNode, int>(root, 0);

            walker.EdgeCrossed += e => distance[e.Right] = distance[e.Left] + 1;

            foreach (TEdge edge in walker.Walk(graph, root))
                yield return new KeyValuePair<TNode, int>(edge.Right, distance[edge.Right]);
        }

        public static IEnumerable<KeyValuePair<TNode, double>> Cost<TNode, TEdge>(this IGraph<TNode, TEdge> graph, TNode root)
            where TEdge : WeightedEdge<TNode>
        {
            var cost = new Dictionary<TNode, double>();
            var walker = new BreadthFirstWalker<TNode, TEdge>();

            cost[root] = 0;

            yield return new KeyValuePair<TNode, double>(root, 0);

            walker.EdgeCrossed += e => cost[e.Right] = cost[e.Left] + e.Weight;

            foreach (TEdge edge in walker.Walk(graph, root))
                yield return new KeyValuePair<TNode, double>(edge.Right, cost[edge.Right]);
        }
    }
}
