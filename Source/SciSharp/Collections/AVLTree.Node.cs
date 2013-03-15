using System;
using System.Collections;
using System.Collections.Generic;


namespace SciSharp.Collections
{
    public partial class AvlTree<T>
    {
        #region Nested type: Node

        protected class Node : IEnumerable<T>
        {
            private readonly IComparer<T> comparer;
            private int balance;
            private int height;

            private Node left;
            private Node parent;
            private Node right;

            public Node(T value, IComparer<T> comparer)
            {
                this.comparer = comparer;
                Value = value;
            }

            public T Value { get; set; }

            public Node Parent
            {
                get { return parent; }
                set { parent = value; }
            }

            public int Balance
            {
                get { return balance; }
            }

            public bool IsLeaf
            {
                get { return left == null && right == null; }
            }

            public bool IsLeftChild
            {
                get { return parent != null && comparer.Compare(Value, parent.Value) < 0; }
            }

            public bool IsRightChild
            {
                get { return parent != null && comparer.Compare(Value, parent.Value) >= 0; }
            }

            public bool IsFull
            {
                get { return left != null && right != null; }
            }

            public int Height
            {
                get { return height; }
            }

            public Node Left
            {
                get { return left; }
                set
                {
                    left = value;

                    if (value != null)
                        value.Parent = this;
                }
            }

            public Node Right
            {
                get { return right; }
                set
                {
                    right = value;

                    if (value != null)
                        value.Parent = this;
                }
            }

            #region IEnumerable<T> Members

            public IEnumerator<T> GetEnumerator()
            {
                if (Left != null)
                    foreach (T item in Left)
                        yield return item;

                yield return Value;

                if (Right != null)
                    foreach (T item in Right)
                        yield return item;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            #endregion

            public void RotateLeft()
            {
                Node root = RotateLeft(new Node(Value, comparer) {Left = Left, Right = Right});

                Value = root.Value;
                Left = root.Left;
                Right = root.Right;
            }

            public void RotateRight()
            {
                Node root = RotateRight(new Node(Value, comparer) {Left = Left, Right = Right});

                Value = root.Value;
                Left = root.Left;
                Right = root.Right;
            }

            public static Node RotateLeft(Node root)
            {
                if (root == null)
                    throw new ArgumentNullException("root");

                Node left = root.Left;

                if (left == null)
                    throw new InvalidProgramException("Cannot rotate a tree that hasn't a left child.");

                root.Left = left.Right;
                left.Right = root;

                root.UpdateStatics();
                left.UpdateStatics();

                return left;
            }

            public static Node RotateRight(Node root)
            {
                if (root == null)
                    throw new ArgumentNullException("root");

                Node right = root.Right;

                if (right == null)
                    throw new InvalidProgramException("Cannot rotate a tree that hasn't a right child.");

                root.Right = right.Left;
                right.Left = root;

                root.UpdateStatics();
                right.UpdateStatics();

                return right;
            }

            public void UpdateStatics()
            {
                int lh = left == null ? -1 : left.height;
                int rh = right == null ? -1 : right.height;

                height = 1 + (lh > rh ? lh : rh);
                balance = rh - lh;
            }

            public void ChildToParent(bool leftChild)
            {
                if (Parent == null)
                    throw new InvalidOperationException("This tree is a root.");

                Node child = leftChild ? left : right;

                if (IsLeftChild)
                    Parent.Left = child;
                else if (IsRightChild)
                    Parent.Right = child;
            }

            public override string ToString()
            {
                return string.Format("[{0}], Height: {1}, Balance: {2}", Value, Height, Balance);
            }
        }

        #endregion
    }
}
