using System;


namespace SciSharp.Geometry
{
    public struct Point2 : IEquatable<Point2>
    {
        #region Constructors

        public Point2(double x, double y)
            : this()
        {
            X = x;
            Y = y;
        }

        #endregion

        #region Properties

        public double X { get; set; }
        public double Y { get; set; }

        public double LengthSqr
        {
            get { return X*X + Y*Y; }
        }

        public double Length
        {
            get { return Math.Sqrt(X*X + Y*Y); }
        }

        public double Angle
        {
            get
            {
                if (Y == 0)
                    return X >= 0 ? 0 : Math.PI;

                if (X == 0)
                    return Y >= 0 ? Math.PI/2 : 3*Math.PI/2;

                double angle = Math.Atan2(Y, X);
                return angle >= 0 ? angle : Math.PI + angle;
            }
        }

        #endregion

        #region IEquatable<Point2> Members

        public bool Equals(Point2 other)
        {
            return other.X.Equals(X) && other.Y.Equals(Y);
        }

        #endregion

        #region Members

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (obj.GetType() != typeof (Point2))
                return false;
            return Equals((Point2) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode()*397) ^ Y.GetHashCode();
            }
        }

        public override string ToString()
        {
            return string.Format("X: {0}, Y: {1}", X, Y);
        }

        /// <summary>
        /// Determines if the segment p0p1 is clockwise from the segment p0p2. 
        /// </summary>
        /// <param name="p0">The origin of both segments.</param>
        /// <param name="p1">The endpoint of the first segment.</param>
        /// <param name="p2">The endpoint of the second segment</param>
        /// <returns>True if p0p1 is clockwise from p0p2, false otherwise.</returns>
        public static bool Clockwise(Point2 p0, Point2 p1, Point2 p2)
        {
            return ((p1 - p0) ^ (p2 - p0)) > 0;
        }

        /// <summary>
        /// Determines if the segment p0p1 is counterclockwise from the segment p0p2. 
        /// </summary>
        /// <param name="p0">The origin of both segments.</param>
        /// <param name="p1">The endpoint of the first segment.</param>
        /// <param name="p2">The endpoint of the second segment</param>
        /// <returns>True if p0p1 is counterclockwise from p0p2, false otherwise.</returns>
        public static bool CounterClockwise(Point2 p0, Point2 p1, Point2 p2)
        {
            return ((p1 - p0) ^ (p2 - p0)) < 0;
        }

        /// <summary>
        /// Determines if walking from the point p0 to the point p2 produces a left turn at point p1.
        /// </summary>
        /// <param name="p0">The origin.</param>
        /// <param name="p1">The turn point.</param>
        /// <param name="p2">The endpoint.</param>
        /// <returns>True if there is a left turn at p1, false otherwise.</returns>
        public static bool LeftTurn(Point2 p0, Point2 p1, Point2 p2)
        {
            return ((p2 - p0) ^ (p1 - p0)) < -Engine.Epsilon;
        }

        /// <summary>
        /// Determines if walking from the point p0 to the point p2 produces a right turn at point p1.
        /// </summary>
        /// <param name="p0">The origin.</param>
        /// <param name="p1">The turn point.</param>
        /// <param name="p2">The endpoint.</param>
        /// <returns>True if there is a right turn at p1, false otherwise.</returns>
        public static bool RightTurn(Point2 p0, Point2 p1, Point2 p2)
        {
            return ((p2 - p0) ^ (p1 - p0)) > Engine.Epsilon;
        }

        public static bool Collinear(Point2 p0, Point2 p1, Point2 p2)
        {
            return Math.Abs((p1 - p0) ^ (p2 - p0)) <= Engine.Epsilon;
        }

        public static bool operator ==(Point2 left, Point2 right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Point2 left, Point2 right)
        {
            return !left.Equals(right);
        }

        public static Point2 operator +(Point2 p1, Point2 p2)
        {
            return new Point2(p1.X + p2.X, p1.Y + p2.Y);
        }

        public static Point2 operator -(Point2 p1, Point2 p2)
        {
            return new Point2(p1.X - p2.X, p1.Y - p2.Y);
        }

        public static double operator *(Point2 p1, Point2 p2)
        {
            return p1.X*p2.X + p1.Y*p2.Y;
        }

        public static Point2 operator *(Point2 p, double d)
        {
            return new Point2(p.X*d, p.Y*d);
        }

        public static Point2 operator *(double d, Point2 p)
        {
            return p*d;
        }

        public static Point2 operator /(Point2 p, double d)
        {
            return new Point2(p.X/d, p.Y/d);
        }

        public static Point2 operator +(Point2 p)
        {
            return p;
        }

        public static Point2 operator -(Point2 p)
        {
            return new Point2(-p.X, -p.Y);
        }

        public static double operator ^(Point2 p1, Point2 p2)
        {
            return p1.X*p2.Y - p1.Y*p2.X;
        }

        /// <summary>
        /// Compares two points by the polar angle of the segment between the origin and the given point.
        /// </summary>
        /// <param name="p1">The first point.</param>
        /// <param name="p2">The second point.</param>
        /// <returns>A value greater than zero if p1 has a greater polar angle than p2, lower than zero if 
        /// p2 has a greater polar angle the p1, or zero if both have the same angle.</returns>
        public static int CompareByPolar(Point2 p1, Point2 p2)
        {
            int cmp = Math.Sign(p2 ^ p1);
            return cmp != 0 ? cmp : p2.LengthSqr.CompareTo(p1.LengthSqr);
        }

        /// <summary>
        /// Constructs a <see cref="Comparison{T}"/> delegate that compares
        /// polar angles formed by segments form the point <paramref name="p0"/> to
        /// the input points of the comparison operation.
        /// </summary>
        /// <param name="p0">The origin of the segments to compare.</param>
        /// <returns>A <see cref="Comparison{T}"/> delegate that performs the required comparison.</returns>
        public static Comparison<Point2> CompareByPolar(Point2 p0)
        {
            return (p1, p2) => CompareByPolar(p1 - p0, p2 - p0);
        }

        public static int CompareX(Point2 p1, Point2 p2)
        {
            int cmp = p1.X.CompareTo(p2.X);
            return cmp != 0 ? cmp : p1.Y.CompareTo(p2.Y);
        }

        public static int CompareY(Point2 p1, Point2 p2)
        {
            int cmp = p1.Y.CompareTo(p2.Y);
            return cmp != 0 ? cmp : p1.X.CompareTo(p2.X);
        }

        #endregion
    }
}
