using System;

using SciSharp.Collections;
using SciSharp.Sorting;


namespace SciSharp.Geometry
{
    public class GrahamScan : OfflineConvexHull2
    {
        #region Instance Fields

        private readonly ISorter<Point2> sorter;
        private readonly IStack<Point2> stack;

        #endregion

        #region Constructors

        public GrahamScan(Point2[] points, IStack<Point2> stack, ISorter<Point2> sorter)
            : base(points)
        {
            if (stack == null)
                throw new ArgumentNullException("stack");

            if (sorter == null)
                throw new ArgumentNullException("sorter");

            if (stack.Count > 0)
                throw new ArgumentException("The stack is not empty.");

            this.stack = stack;
            this.sorter = sorter;
        }

        #endregion

        #region Members

        public override Point2[] Solve()
        {
            // Put on p0 the lowest leftmost point.
            int p0 = 0;

            // Find the lowest point, breaking ties by the leftmost one.
            for (int i = 1; i < Points.Length; i++)
                if (Point2.CompareY(Points[i], Points[p0]) < 0)
                    p0 = i;

            Points.Swap(p0, 0);

            // Sort the points according to their polar angles with respect to p0.
            sorter.Sort(Points, 1, Point2.CompareByPolar(Points[0]));

            // Get the convex hull of the first 3 points.
            stack.Push(Points[0]);
            stack.Push(Points[1]);
            stack.Push(Points[2]);

            // Invariant : The stack always contains the convex hull of the points p0 - pi
            for (int i = 3; i < Points.Length; stack.Push(Points[i++]))
                // Remove all the points interior to the hull.
                while (stack.Count > 1 && Point2.RightTurn(stack[1], stack[0], Points[i]))
                    stack.Pop();

            var result = new Point2[stack.Count];

            int p = stack.Count - 1;

            // Return the points from bottom to top of the stack.
            while (stack.Count > 0)
                result[p--] = stack.Pop();

            return result;
        }

        #endregion
    }
}
