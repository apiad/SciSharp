using System;


namespace SciSharp.Collections
{
    /// <summary>
    /// Represents an undirected or directed edge going from <see cref="Left"/> to <see cref="Right"/>.
    /// </summary>
    /// <typeparam name="TNode">The type of the node.</typeparam>
    public class Edge<TNode> : IEquatable<Edge<TNode>>
    {
        public Edge(TNode left, TNode right)
        {
            Left = left;
            Right = right;
        }

        /// <summary>
        /// The left or origin node.
        /// </summary>
        public TNode Left { get; internal set; }

        /// <summary>
        /// The right or destination node.
        /// </summary>
        public TNode Right { get; internal set; }

        #region IEquatable<Edge<TNode>> Members

        public bool Equals(Edge<TNode> other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return Equals(other.Left, Left) && Equals(other.Right, Right);
        }

        #endregion

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != typeof (Edge<TNode>))
                return false;
            return Equals((Edge<TNode>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Left.GetHashCode()*397) ^ Right.GetHashCode();
            }
        }

        public static bool operator ==(Edge<TNode> left, Edge<TNode> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Edge<TNode> left, Edge<TNode> right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return string.Format("({0}, {1})", Left, Right);
        }
    }
}
