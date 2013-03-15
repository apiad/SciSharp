namespace SciSharp.Collections
{
    public interface IBinaryTree<T> : ITree<T>
    {
        IBinaryTree<T> Left { get; }
        IBinaryTree<T> Right { get; }
    }
}
