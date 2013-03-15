using System;
using System.Collections;
using System.Collections.Generic;


namespace SciSharp.Collections
{
    /// <summary>
    /// Represents a randomized binary minimum heap.
    /// 
    /// A randomized heap is built using a single merge operation
    /// between two randomized sub-heaps. It maintains the min heap
    /// invariant by selecting as root the smallest of the sub-heaps
    /// roots, and recursively merging the rest of the heaps. 
    /// It keeps an expected logarithmic height by randomly
    /// swapping the left and right nodes on the other sub-heap 
    /// (whose root was the biggest), thus probabilistically 
    /// avoiding all ill-conditioned configurations.
    /// </summary>
    /// 
    /// <remarks>
    /// The randomized heap has logarithmic expected cost for
    /// all operations. However, in very rare cases it can perform
    /// sub-optimally, if unlucky enough to always merge with the 
    /// bigger heap. This is a very unlikey event, which depends
    /// on the random generator. If you find the heap having a non-logarithmic
    /// height (> 2 * log(n)), consider calling the <see cref="RandomizedHeap{T}.Rebuild"/>,
    /// a linear operation which rebuilds the entire heap. It is extremely unlikely
    /// that the heap stays linear after this operation.
    /// 
    /// Given the low overhead of the randomized heap implementation,
    /// in practice it can perform faster than other deterministic
    /// heap implementations such as <see cref="BinaryHeap{T}"/>.
    /// </remarks>
    /// 
    /// <typeparam name="T">The type of the items stored in the heap.</typeparam>
    /// 
    /// <seealso cref="BinaryHeap{T}">
    /// A deterministic binary heap with the same asymptotic cost for
    /// all operations.
    /// </seealso>
    public class RandomizedHeap<T> : IQueue<T>, IEnumerable<T>
        where T : IComparable<T>
    {
        #region Static and Constant Fields

        private static Random Random = new Random();

        #endregion

        #region Instance Fields

        private Node root;

        #endregion

        #region Constructors

        public RandomizedHeap() {}

        public RandomizedHeap(T value)
        {
            root = new Node(value);
            Count = 1;
        }

        private RandomizedHeap(Node root, int count)
        {
            Count = count;
            this.root = root;
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return root.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region IQueue<T> Members

        void IQueue<T>.Enqueue(T item)
        {
            Add(item);
        }

        public T Peek()
        {
            if (root == null)
                throw new InvalidOperationException("The heap is empty.");

            return root.Value;
        }

        T IQueue<T>.Dequeue()
        {
            return Pop();
        }

        public int Count { get; private set; }

        #endregion

        #region Members

        public void Add(T item)
        {
            root = Node.Merge(root, new Node(item));
            Count++;
        }

        public T Pop()
        {
            if (root == null)
                throw new InvalidOperationException("The heap is empty.");

            T t = root.Value;

            root = Node.Merge(root.Left, root.Right);
            Count--;

            return t;
        }

        public void Merge(RandomizedHeap<T> other)
        {
            root = Node.Merge(root, other.root);
            Count += other.Count;
        }

        public void Rebuild()
        {
            var items = new LinkedList<T>(this);

            root = null;
            Count = 0;

            foreach (T item in items)
                Add(item);
        }

        public static RandomizedHeap<T> Merge(RandomizedHeap<T> left, RandomizedHeap<T> right)
        {
            if (left == null)
                throw new ArgumentNullException("left");

            if (right == null)
                throw new ArgumentNullException("right");

            return new RandomizedHeap<T>(Node.Merge(left.root, right.root), left.Count + right.Count);
        }

        #endregion

        #region Nested type: Node

        private class Node : IEnumerable<T>
        {
            #region Constructors

            public Node(T value)
            {
                Value = value;
            }

            #endregion

            #region Properties

            public T Value { get; private set; }

            public Node Left { get; private set; }

            public Node Right { get; private set; }

            #endregion

            #region Members

            public static Node Merge(Node left, Node right)
            {
                if (left == null || right == null)
                    return left ?? right;

                if (right.Value.CompareTo(left.Value) < 0)
                {
                    Node tmp = left;
                    left = right;
                    right = tmp;
                }

                if ((Random.Next() & 1) == 0)
                {
                    Node tmp = left.Left;
                    left.Left = left.Right;
                    left.Right = tmp;
                }

                left.Left = Merge(left.Left, right);

                return left;
            }

            #endregion

            #region IEnumerable<T> Members

            public IEnumerator<T> GetEnumerator()
            {
                yield return Value;

                if (Left != null)
                    foreach (T item in Left)
                        yield return item;

                if (Right != null)
                    foreach (T item in Right)
                        yield return item;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            #endregion
        }

        #endregion
    }
}
