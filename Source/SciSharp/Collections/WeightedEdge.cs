using System;


namespace SciSharp.Collections
{
    public class WeightedEdge<TNode> : Edge<TNode>, IEquatable<WeightedEdge<TNode>>
    {
        public WeightedEdge(TNode left, TNode right, double weight)
            : base(left, right)
        {
            Weight = weight;
        }

        public double Weight { get; set; }

        #region IEquatable<WeightedEdge<TNode>> Members

        public bool Equals(WeightedEdge<TNode> other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return base.Equals(other) && other.Weight.Equals(Weight);
        }

        #endregion

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            return Equals(obj as WeightedEdge<TNode>);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode()*397) ^ Weight.GetHashCode();
            }
        }

        public static bool operator ==(WeightedEdge<TNode> left, WeightedEdge<TNode> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(WeightedEdge<TNode> left, WeightedEdge<TNode> right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return string.Format("{0}, Weight: {1}", base.ToString(), Weight);
        }
    }
}
