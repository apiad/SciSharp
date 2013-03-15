using System.Collections.Generic;


namespace SciSharp.Collections.Pathfinding
{
    public class BreadthFirstWalker<TNode, TEdge> : GraphWalker<TNode, TEdge>
        where TEdge : Edge<TNode>
    {
        public override IEnumerable<TEdge> Walk(IGraph<TNode, TEdge> graph, TNode root)
        {
            var queue = new Queue<TNode>();

            queue.Enqueue(root);
            OpenNode(root);

            while (queue.Count > 0)
            {
                TNode node = queue.Dequeue();

                foreach (TNode child in graph.AdyacentOf(node))
                {
                    if (Visited(child))
                        continue;

                    TEdge edge = graph[node, child];
                    CrossEdge(edge);
                    yield return edge;

                    queue.Enqueue(child);
                    OpenNode(child);
                }

                CloseNode(node);
            }
        }
    }
}
