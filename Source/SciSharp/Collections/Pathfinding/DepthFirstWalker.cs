using System.Collections.Generic;


namespace SciSharp.Collections.Pathfinding
{
    public class DepthFirstWalker<TNode, TEdge> : GraphWalker<TNode, TEdge>
        where TEdge : Edge<TNode>
    {
        public override IEnumerable<TEdge> Walk(IGraph<TNode, TEdge> graph, TNode root)
        {
            OpenNode(root);

            foreach (TNode node in graph.AdyacentOf(root))
            {
                if (!Open(node))
                {
                    CrossEdge(graph[root, node]);
                    yield return graph[root, node];

                    foreach (TEdge edge in Walk(graph, node))
                        yield return edge;
                }
            }

            CloseNode(root);
        }
    }
}
