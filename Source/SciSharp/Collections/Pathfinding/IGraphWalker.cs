using System;
using System.Collections.Generic;


namespace SciSharp.Collections.Pathfinding
{
    public interface IGraphWalker<TNode, TEdge>
        where TEdge : Edge<TNode>
    {
        IEnumerable<TEdge> Walk(IGraph<TNode, TEdge> graph, TNode root);

        event Action<TNode> NodeOpen;
        event Action<TNode> NodeClosed;
        event Action<TEdge> EdgeCrossed;
    }
}
