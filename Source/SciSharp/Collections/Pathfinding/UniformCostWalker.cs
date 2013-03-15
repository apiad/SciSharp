namespace SciSharp.Collections.Pathfinding
{
    public class UniformCostWalker<TNode, TEdge> : AStarWalker<TNode, TEdge>
        where TEdge : WeightedEdge<TNode>
    {
        public UniformCostWalker()
            : base(node => 0) {}
    }
}
