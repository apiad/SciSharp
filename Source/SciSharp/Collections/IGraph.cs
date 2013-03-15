using System.Collections.Generic;


namespace SciSharp.Collections
{
    public interface IGraph<TNode, TEdge>
        where TEdge : Edge<TNode>
    {
        TEdge this[TNode u, TNode v] { get; set; }

        IEnumerable<TNode> AdyacentOf(TNode node);

        int Degree(TNode node);
    }

    public interface IGraph<T> : IGraph<T, Edge<T>> {}
}
