using System.Collections.Generic;

using SciSharp.Collections;


namespace SciSharp.Geometry
{
    public class JarvisMarch : OfflineConvexHull2
    {
        #region Constructors

        public JarvisMarch(Point2[] points)
            : base(points) {}

        #endregion

        #region Members

        public override Point2[] Solve()
        {
            // Put on p0 the lower left point.
            int p0 = 0;

            // Find the leftmost point, breaking ties by the lowest one.
            for (int i = 1; i < Points.Length; i++)
                if (Point2.CompareX(Points[i], Points[p0]) < 0)
                    p0 = i;

            Points.Swap(p0, 0);

            // List of points of the convex hull.
            var hull = new List<Point2>();

            // Holds the current point in the hull.
            int curr = 0;

            do
            {
                // Add the current point to the hull.
                hull.Add(Points[curr]);
                int next = -1;

                for (int i = 0; i < Points.Length; i++)
                {
                    // Skip the current point.
                    if (i == curr)
                        continue;

                    // Update the first time.
                    if (next == -1)
                        next = i;

                    // Keep the point with lowest polar angle with respect to curr.
                    if (Point2.CompareByPolar(Points[i] - Points[curr], Points[next] - Points[curr]) < 0)
                        next = i;
                }

                // This is the next point in the hull.
                curr = next;
            } while (curr != 0);

            // Return the value.
            return hull.ToArray();
        }

        #endregion
    }
}
