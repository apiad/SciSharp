using System;
using System.Collections.Generic;

using SciSharp.Probabilities;


namespace SciSharp.Collections.Pathfinding
{
    public class RandomizedGreedyWalker<TNode, TEdge> : GraphWalker<TNode, TEdge>
        where TEdge : Edge<TNode>
    {
        protected readonly int MaxDepth;
        protected double Chance;
        protected Func<TNode, double> Heuristic;

        public RandomizedGreedyWalker(Func<TNode, double> heuristic, int maxDepth, double chanceToChange = 0.5d)
        {
            Chance = chanceToChange;
            Heuristic = heuristic;
            MaxDepth = maxDepth;
        }

        public override IEnumerable<TEdge> Walk(IGraph<TNode, TEdge> graph, TNode root)
        {
            var rdm = new RandomEx();
            var fringe = new BinaryHeap<TNode>((n1, n2) => Heuristic(n1).CompareTo(Heuristic(n2))) {root};
            var depths = new Dictionary<TNode, int> {{root, 0}};

            OpenNode(root);
            bool first = true;

            while (!fringe.Empty)
            {
                TNode node = fringe.Extract();
                bool change = rdm.Bernoulli(Chance);
                var store = new LinkedList<TNode>();

                while (change && !fringe.Empty)
                {
                    store.AddLast(node);
                    node = fringe.Extract();
                    change = rdm.Bernoulli(Chance);
                }

                foreach (TNode n in store)
                    fringe.Add(n);

                TEdge e = default(TEdge);
                int depth = first ? 0 : depths[node] = depths[(e = Crossings[node]).Left] + 1;

                if (depth >= MaxDepth)
                    yield break;

                if (!first)
                    yield return e;

                first = false;

                foreach (TNode child in graph.AdyacentOf(node))
                {
                    TEdge edge = graph[node, child];

                    if (!Visited(child))
                    {
                        CrossEdge(edge);
                        fringe.Add(child);
                        OpenNode(child);
                    }
                }

                CloseNode(node);
            }
        }
    }
}
