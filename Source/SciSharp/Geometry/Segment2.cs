using System;


namespace SciSharp.Geometry
{
    public struct Segment2 : IEquatable<Segment2>
    {
        #region Constructors

        public Segment2(Point2 a, Point2 b) : this()
        {
            A = a;
            B = b;
        }

        #endregion

        #region Properties

        public Point2 A { get; set; }
        public Point2 B { get; set; }

        #endregion

        #region IEquatable<Segment2> Members

        public bool Equals(Segment2 other)
        {
            return other.A.Equals(A) && other.B.Equals(B);
        }

        #endregion

        #region Members

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (obj.GetType() != typeof (Segment2))
                return false;
            return Equals((Segment2) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (A.GetHashCode()*397) ^ B.GetHashCode();
            }
        }

        public int Side(Point2 p)
        {
            Point2 ab = B - A;
            Point2 ap = p - A;

            return Math.Sign(ab ^ ap);
        }

        public double PseudoDistance(Point2 p)
        {
            Point2 ab = B - A;
            Point2 pa = A - p;

            return Math.Abs(ab ^ pa);
        }

        public static bool operator ==(Segment2 left, Segment2 right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Segment2 left, Segment2 right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return string.Format("A: {0}, B: {1}", A, B);
        }

        #endregion
    }
}
