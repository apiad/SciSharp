using System.Collections.Generic;


namespace SciSharp.Collections
{
    public interface IDirectedGraph<TNode, TEdge> : IGraph<TNode, TEdge>
        where TEdge : Edge<TNode>
    {
        IEnumerable<TNode> InNeighboors(TNode node);

        IEnumerable<TNode> OutNeighboors(TNode node);
    }
}
