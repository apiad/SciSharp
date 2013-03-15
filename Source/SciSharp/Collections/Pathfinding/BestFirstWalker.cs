using System;
using System.Collections.Generic;


namespace SciSharp.Collections.Pathfinding
{
    public class BestFirstWalker<TNode, TEdge> : GraphWalker<TNode, TEdge>
        where TEdge : Edge<TNode>
    {
        protected Func<TNode, double> Cost;

        public BestFirstWalker(Func<TNode, double> cost)
        {
            Cost = cost;
        }

        protected BestFirstWalker() {}

        public override IEnumerable<TEdge> Walk(IGraph<TNode, TEdge> graph, TNode root)
        {
            var fringe = new BinaryHeap<TNode>((n1, n2) => Cost(n1).CompareTo(Cost(n2))) {root};

            OpenNode(root);

            while (!fringe.Empty)
            {
                TNode node = fringe.Extract();

                if (!node.Equals(root))
                    yield return Crossings[node];

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
