using System;


namespace SciSharp.Collections.Pathfinding
{
    public class GreedyWalker<TNode, TEdge> : BestFirstWalker<TNode, TEdge>
        where TEdge : Edge<TNode>
    {
        public GreedyWalker(Func<TNode, double> heuristic)
            : base(n => heuristic(n)) {}
    }
}
