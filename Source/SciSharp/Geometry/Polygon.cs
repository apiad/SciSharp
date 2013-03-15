using System;
using System.Collections.Generic;


namespace SciSharp.Geometry
{
    public class Polygon
    {
        #region Members

        public static Point2[] RemoveCollinear(Point2[] points)
        {
            if (points == null)
                throw new ArgumentNullException("points");

            var result = new List<Point2>(points.Length) {points[0]};

            for (int i = 1; i < points.Length - 1; i++)
                if (!Point2.Collinear(result[result.Count - 1], points[i], points[i + 1]))
                    result.Add(points[i]);

            if (!Point2.Collinear(result[result.Count - 1], points[points.Length - 1], points[0]))
                result.Add(points[points.Length - 1]);

            return result.ToArray();
        }

        #endregion
    }
}
