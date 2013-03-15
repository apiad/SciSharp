using System;
using System.Collections.Generic;
using System.Linq;


namespace SciSharp.Collections.Pathfinding
{
    public class BidirectionalGraphWalker<TNode, TEdge>
        where TEdge : Edge<TNode>
    {
        private readonly GraphWalker<TNode, TEdge> destinationWalker;
        private readonly GraphWalker<TNode, TEdge> originWalker;
        private TNode end;
        private bool found;
        private TNode middleDestination;
        private TNode middleOrigin;
        private TNode start;

        public BidirectionalGraphWalker(GraphWalker<TNode, TEdge> originWalker,
                                        GraphWalker<TNode, TEdge> destinationWalker)
        {
            if (originWalker == null)
                throw new ArgumentNullException("originWalker");

            if (destinationWalker == null)
                throw new ArgumentNullException("destinationWalker");

            this.originWalker = originWalker;
            this.destinationWalker = destinationWalker;

            originWalker.EdgeCrossed += e =>
                                        {
                                            if (destinationWalker.Visited(e.Right))
                                            {
                                                found = true;
                                                middleOrigin = e.Left;
                                                middleDestination = e.Right;
                                            }
                                        };

            destinationWalker.EdgeCrossed += e =>
                                             {
                                                 if (originWalker.Visited(e.Right))
                                                 {
                                                     found = true;
                                                     middleOrigin = e.Right;
                                                     middleDestination = e.Left;
                                                 }
                                             };
        }

        public bool Found
        {
            get { return found; }
        }

        public int Hits
        {
            get { return originWalker.Hits + destinationWalker.Hits; }
        }

        public IEnumerable<TNode> Path
        {
            get
            {
                if (!found)
                    throw new InvalidOperationException("Not path was found.");

                if (start.Equals(end))
                {
                    yield return start;
                    yield break;
                }

                var walk1 = new Walk<TNode, TEdge>(originWalker.Crossings);
                var walk2 = new Walk<TNode, TEdge>(destinationWalker.Crossings);

                IEnumerable<TNode> path1 = walk1.Path(start, middleOrigin);
                IEnumerable<TNode> path2 = walk2.Path(end, middleDestination);

                foreach (TNode node in path1)
                    yield return node;

                foreach (TNode node in path2.Reverse())
                    yield return node;
            }
        }

        public IEnumerable<TEdge> Walk(IGraph<TNode, TEdge> graph, TNode start, TNode end)
        {
            found = false;

            this.start = start;
            this.end = end;

            IEnumerator<TEdge> fromOrigin = originWalker.Walk(graph, start).GetEnumerator();
            IEnumerator<TEdge> fromDestination = destinationWalker.Walk(graph, end).GetEnumerator();

            if (start.Equals(end))
            {
                found = true;
                middleOrigin = start;
                middleDestination = end;
                yield break;
            }

            bool m1 = true, m2 = true;

            while (m1 || m2)
            {
                m1 = fromOrigin.MoveNext();

                if (found)
                    yield break;

                yield return fromOrigin.Current;

                m2 = fromDestination.MoveNext();

                if (found)
                    yield break;

                yield return fromDestination.Current;
            }
        }
    }
}
