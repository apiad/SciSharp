using System;


namespace SciSharp.Geometry
{
    public abstract class OfflineConvexHull2
    {
        #region Instance Fields

        protected readonly Point2[] Points;

        #endregion

        #region Constructors

        protected OfflineConvexHull2(Point2[] points)
        {
            if (points == null)
                throw new ArgumentNullException("points");

            if (points.Length < 3)
                throw new ArgumentOutOfRangeException("points",
                                                      "The length of the array must be greater than 2 to form a polygon.");

            Points = points;
        }

        #endregion

        #region Members

        public abstract Point2[] Solve();

        protected int UpperFarthest()
        {
            // Upper and farthest point.
            int upper = 0;

            // Find the upper point breaking ties by the farthest from p0.
            for (int i = 0; i < Points.Length; i++)
                if (Points[i].Y > Points[upper].Y || Points[i].Y == Points[upper].Y &&
                    (Points[i] - Points[0]).LengthSqr > (Points[upper] - Points[0]).LengthSqr)
                    upper = i;

            return upper;
        }

        #endregion
    }
}
