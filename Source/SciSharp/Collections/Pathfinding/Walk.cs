using System.Collections.Generic;


namespace SciSharp.Collections.Pathfinding
{
    public class Walk<TNode, TEdge>
        where TEdge : Edge<TNode>
    {
        private readonly IDictionary<TNode, TEdge> crossings;

        public Walk(IEnumerable<TEdge> edges)
        {
            crossings = new Dictionary<TNode, TEdge>();

            foreach (TEdge edge in edges)
                if (!crossings.ContainsKey(edge.Right))
                    crossings[edge.Right] = edge;
        }

        public Walk()
        {
            crossings = new Dictionary<TNode, TEdge>();
        }

        public Walk(IDictionary<TNode, TEdge> crossings)
        {
            this.crossings = crossings;
        }

        public IEnumerable<TNode> Nodes
        {
            get { return crossings.Keys; }
        }

        public int Count
        {
            get { return crossings.Count; }
        }

        public TEdge this[TNode node]
        {
            get { return crossings[node]; }
        }

        public IEnumerable<TNode> Path(TNode start, TNode end)
        {
            var list = new LinkedList<TEdge>();
            TEdge e = crossings[end];

            while (!end.Equals(start))
            {
                list.AddFirst(e);
                e = crossings[end];
                end = e.Left;
            }

            yield return list.First.Value.Left;

            foreach (TEdge edge in list)
                yield return edge.Right;
        }

        public IEnumerable<TEdge> Edges(TNode start, TNode end)
        {
            var list = new LinkedList<TEdge>();
            TEdge e = crossings[end];

            while (!end.Equals(start))
            {
                list.AddFirst(e);
                e = crossings[end];
                end = e.Left;
            }

            foreach (TEdge edge in list)
                yield return edge;
        }

        public void Augment(Walk<TNode, TEdge> rest)
        {
            foreach (var pair in rest.crossings)
                if (!crossings.ContainsKey(pair.Key))
                    crossings[pair.Key] = pair.Value;
        }

        public bool Contains(TNode node)
        {
            return crossings.ContainsKey(node);
        }
    }
}
