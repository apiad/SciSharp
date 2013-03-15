using System;

using SciSharp.Collections;


namespace SciSharp.Geometry
{
    public abstract class OfflineClosestPair2
    {
        #region Instance Fields

        protected readonly Point2[] Points;

        #endregion

        #region Constructors

        protected OfflineClosestPair2(Point2[] points)
        {
            if (points == null)
                throw new ArgumentNullException("points");

            if (points.Length < 2)
                throw new ArgumentException("The array must contain at least two elements.");

            Points = points;
        }

        protected OfflineClosestPair2(IFiniteSet<Point2> points)
        {
            if (points == null)
                throw new ArgumentNullException("points");

            if (points.Count < 2)
                throw new ArgumentException("The array must contain at least two elements.");

            Points = points.ToArray();
        }

        #endregion

        #region Members

        public abstract Pair<Point2> Solve();

        #endregion
    }
}
