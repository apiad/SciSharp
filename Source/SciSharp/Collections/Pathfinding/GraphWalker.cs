using System;
using System.Collections.Generic;


namespace SciSharp.Collections.Pathfinding
{
    public abstract class GraphWalker<TNode, TEdge> : IGraphWalker<TNode, TEdge>
        where TEdge : Edge<TNode>
    {
        private readonly ICollection<TNode> closed;
        private readonly IDictionary<TNode, TEdge> crossings;
        private readonly ICollection<TNode> open;

        protected GraphWalker(ICollection<TNode> open, ICollection<TNode> closed)
        {
            this.open = open;
            this.closed = closed;
            crossings = new Dictionary<TNode, TEdge>();
        }

        protected GraphWalker()
        {
            open = new HashSet<TNode>();
            closed = new HashSet<TNode>();
            crossings = new Dictionary<TNode, TEdge>();
        }

        public int Hits { get; set; }

        public IDictionary<TNode, TEdge> Crossings
        {
            get { return crossings; }
        }

        #region IGraphWalker<TNode,TEdge> Members

        public abstract IEnumerable<TEdge> Walk(IGraph<TNode, TEdge> graph, TNode root);

        public event Action<TNode> NodeOpen;

        public event Action<TNode> NodeClosed;

        public event Action<TEdge> EdgeCrossed;

        #endregion

        protected virtual void OpenNode(TNode node)
        {
            open.Add(node);

            Action<TNode> handler = NodeOpen;

            if (handler != null)
                handler(node);
        }

        protected virtual void CloseNode(TNode node)
        {
            open.Remove(node);
            closed.Add(node);

            Action<TNode> handler = NodeClosed;

            if (handler != null)
                handler(node);
        }

        protected virtual void CrossEdge(TEdge edge)
        {
            crossings[edge.Right] = edge;

            Action<TEdge> handler = EdgeCrossed;

            if (handler != null)
                handler(edge);
        }

        protected bool Open(TNode node)
        {
            return open.Contains(node);
        }

        protected bool Closed(TNode node)
        {
            return closed.Contains(node);
        }

        public bool Visited(TNode node)
        {
            Hits++;
            return Open(node) || Closed(node);
        }
    }
}
