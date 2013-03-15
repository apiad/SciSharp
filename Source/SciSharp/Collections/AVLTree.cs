using System;
using System.Collections;
using System.Collections.Generic;


namespace SciSharp.Collections
{
    public partial class AvlTree<T> : ICollection<T>
    {
        public readonly IComparer<T> Comparer = Comparer<T>.Default;
        private int count;
        private Node root;
        private int version;

        public AvlTree(IComparer<T> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            Comparer = comparer;
        }

        public AvlTree() {}

        public AvlTree(Comparison<T> cmp)
        {
            if (cmp == null)
                throw new ArgumentNullException("cmp");

            Comparer = new MethodComparer<T>(cmp);
        }

        protected Node Root
        {
            get { return root; }
            private set
            {
                if (value != null)
                    value.Parent = null;

                root = value;
            }
        }

        public int Height
        {
            get { return Root == null ? -1 : Root.Height; }
        }

        public T Min
        {
            get
            {
                if (Root == null)
                    throw new InvalidOperationException("The collection is empty.");

                Node node = Root;

                while (node.Left != null)
                    node = node.Left;

                return node.Value;
            }
        }

        public T Max
        {
            get
            {
                if (Root == null)
                    throw new InvalidOperationException("The collection is empty.");

                Node node = Root;

                while (node.Right != null)
                    node = node.Right;

                return node.Value;
            }
        }

        #region ICollection<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            int ver = version;

            if (Root != null)
                foreach (T item in Root)
                {
                    if (ver != version)
                        throw new InvalidOperationException("The collection has been modified.");

                    yield return item;
                }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Clear()
        {
            root = null;
            version++;
            count = 0;
        }

        public bool Contains(T item)
        {
            Node node = SearchNode(item);

            return node != null && Comparer.Compare(node.Value, item) == 0;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            foreach (T item in this)
                array[arrayIndex++] = item;
        }

        public int Count
        {
            get { return count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public void Add(T item)
        {
            if (Root == null)
                Root = new Node(item, Comparer);
            else
            {
                Node tree = SearchNode(item);

                if (Comparer.Compare(tree.Value, item) == 0)
                    throw new InvalidOperationException("The key already exists.");

                if (Comparer.Compare(item, tree.Value) < 0)
                    tree.Left = new Node(item, Comparer);
                else
                    tree.Right = new Node(item, Comparer);

                while (tree != null)
                {
                    tree.UpdateStatics();

                    if (tree.Balance > 1)
                    {
                        if (tree.Right.Balance < 0)
                            tree.Right.RotateLeft();

                        tree.RotateRight();
                    }
                    else if (tree.Balance < -1)
                    {
                        if (tree.Left.Balance > 0)
                            tree.Left.RotateRight();

                        tree.RotateLeft();
                    }

                    if (tree.Parent == null)
                        Root = tree;

                    tree = tree.Parent;
                }
            }

            version++;
            count++;

            OnElementAdded(item);
        }

        public bool Remove(T item)
        {
            T removed;

            if (Count == 0)
                return false;

            if (Count == 1)
            {
                if (Comparer.Compare(Root.Value, item) == 0)
                {
                    removed = Root.Value;

                    Root = null;
                    count = 0;

                    OnElementRemoved(removed);

                    return true;
                }

                return false;
            }

            Node tree = SearchNode(item);

            if (Comparer.Compare(tree.Value, item) != 0)
                return false;

            removed = tree.Value;

            if (tree.IsFull)
            {
                Node minor = GreaterMinor(tree);
                minor.ChildToParent(true);
                tree.Value = minor.Value;
                tree = minor.Parent;
            }
            else
            {
                Node temp = tree;

                tree = tree.Left ?? tree.Right;

                if (temp.IsLeftChild)
                    temp.Parent.Left = tree;
                else if (temp.IsRightChild)
                    temp.Parent.Right = tree;
                else
                    Root = tree;

                tree = temp.Parent;
            }

            while (tree != null)
            {
                tree.UpdateStatics();

                if (tree.Balance > 1)
                {
                    if (tree.Right.Balance < 0)
                        tree.Right.RotateLeft();

                    tree.RotateRight();
                }
                else if (tree.Balance < -1)
                {
                    if (tree.Left.Balance > 0)
                        tree.Left.RotateRight();

                    tree.RotateLeft();
                }

                if (tree.Parent == null)
                    Root = tree;

                tree = tree.Parent;
            }

            version++;
            count--;

            OnElementRemoved(removed);

            return true;
        }

        #endregion

        public bool TryAdd(T value)
        {
            if (Contains(value))
                return false;

            Add(value);

            return true;
        }

        private Node SearchNode(T item)
        {
            if (Root == null)
                return null;

            Node node = Root;
            int cmp = Comparer.Compare(node.Value, item);

            while (cmp != 0)
            {
                if (cmp < 0)
                {
                    if (node.Left == null)
                        break;

                    node = node.Left;
                }
                else
                {
                    if (node.Right == null)
                        break;

                    node = node.Right;
                }

                cmp = Comparer.Compare(node.Value, item);
            }

            return node;
        }

        private static Node GreaterMinor(Node root)
        {
            if (root == null)
                throw new ArgumentNullException("root");

            Node tree = root.Left;

            if (tree == null)
                return null;

            while (tree.Right != null)
                tree = tree.Right;

            return tree;
        }

        public T RemoveMin()
        {
            T min = Min;
            Remove(min);
            return min;
        }

        public T RemoveMax()
        {
            T max = Max;
            Remove(max);
            return max;
        }

        public override string ToString()
        {
            return string.Format("Count: {0}", Count);
        }

        public event Action<T> ElementAdded;

        protected virtual void OnElementAdded(T item)
        {
            Action<T> handler = ElementAdded;
            if (handler != null)
                handler(item);
        }

        public event Action<T> ElementRemoved;

        protected virtual void OnElementRemoved(T item)
        {
            Action<T> handler = ElementRemoved;
            if (handler != null)
                handler(item);
        }
    }
}
