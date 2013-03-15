using System;
using System.Collections.Generic;


namespace SciSharp.Collections.Pathfinding
{
    public class AStarWalker<TNode, TEdge> : BestFirstWalker<TNode, TEdge>
        where TEdge : WeightedEdge<TNode>
    {
        protected readonly Func<TNode, double> Heuristic;
        protected readonly IDictionary<TNode, double> PathCost = new DefaultDictionary<TNode, double>();

        public AStarWalker(Func<TNode, double> heuristic)
        {
            Heuristic = heuristic;
            Cost = node => PathCost[node] + Heuristic(node);
        }

        protected override void CrossEdge(TEdge edge)
        {
            PathCost[edge.Right] = PathCost[edge.Left] + edge.Weight;

            base.CrossEdge(edge);
        }
    }
}
