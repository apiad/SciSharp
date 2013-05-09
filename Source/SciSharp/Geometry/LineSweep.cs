using System;

using SciSharp.Sorting;


namespace SciSharp.Geometry
{
    public class LineSweep : OfflineClosestPair2
    {
        #region Instance Fields

        private readonly ISorter<int> intSorter;
        private readonly ISorter<Point2> pointSorter;

        #endregion

        #region Constructors

        public LineSweep(Point2[] points, ISorter<Point2> pointSorter, ISorter<int> intSorter)
            : base(points)
        {
            this.pointSorter = pointSorter;
            this.intSorter = intSorter;
        }

        #endregion

        #region Properties

        public bool DoEvents { get; set; }

        #endregion

        #region Members

        public override Pair<Point2> Solve()
        {
            var idy = new int[Points.Length];

            for (int i = 0; i < idy.Length; i++)
            {
                idy[i] = i;
            }

            pointSorter.Sort(Points, Point2.CompareX);
            intSorter.Sort(idy, (i, j) => Point2.CompareY(Points[i], Points[j]));

            var ys = new int[idy.Length];

            for (int i = 0; i < ys.Length; i++)
                ys[idy[i]] = i;

            return ClosestPair(Points, ys);
        }

        private Pair<Point2> ClosestPair(Point2[] points, int[] ys)
        {
            if (points.Length <= 3)
                return CompareAll(points);

            int mid = points.Length / 2;
            double x = points[points.Length / 2].X;

            var left = new Point2[mid];
            var yl = new int[mid];

            var right = new Point2[points.Length - mid];
            var yr = new int[points.Length - mid];

            for (int i = 0; i < left.Length; i++)
                left[i] = points[i];

            for (int i = 0; i < right.Length; i++)
                right[i] = points[i + mid];

            for (int i = 0, l = 0, r = 0; i < points.Length; i++)
                if (ys[i] < mid)
                    yl[l++] = ys[i];
                else
                    yr[r++] = ys[i] - mid;

            Pair<Point2> leftMin = ClosestPair(left, yl);
            Pair<Point2> rightMin = ClosestPair(right, yr);

            Pair<Point2> min = (leftMin.A - leftMin.B).LengthSqr < (rightMin.A - rightMin.B).LengthSqr ? leftMin : rightMin;

            if (DoEvents)
                OnStep(new LineSweepEventArgs(min, 0,0,0,0, points, rightMin, leftMin, min));

            double delta = (min.A = min.B).LengthSqr;

            var y = new int[ys.Length];
            int k = 0;

            for (int i = 0; i < ys.Length; i++)
                if (points[ys[i]].X > x - delta && points[ys[i]].X < x + delta)
                    y[k++] = ys[i];

            for (int i = 0; i < k; i++)
                for (int j = i + 1; j < i + 8 && j < k; j++)
                {
                    double newDelta = (points[i] - points[j]).LengthSqr;

                    if (newDelta < delta)
                    {
                        min = new Pair<Point2>(points[i], points[j]);
                        delta = newDelta;
                    }

                    if (DoEvents)
                        OnStep(new LineSweepEventArgs(min, 0, 0, 0, 0, points, rightMin, leftMin, new Pair<Point2>(points[i], points[j])));
                }

            return min;
        }

        private Pair<Point2> CompareAll(Point2[] points)
        {
            Point2 p1 = points[0];
            Point2 p2 = points[1];

            for (int i = 0; i < points.Length; i++)
                for (int j = 0; j < points.Length; j++)
                {
                    if (i == j)
                        continue;

                    if (DoEvents)
                        OnStep(new LineSweepEventArgs(new Pair<Point2>(p1, p2), 0, 0, 0, 0, points, null, null, new Pair<Point2>(points[i], points[j])));

                    if ((points[i] - points[j]).LengthSqr < (p1 - p2).LengthSqr)
                    {
                        p1 = points[i];
                        p2 = points[j];
                    }
                }

            var pair = new Pair<Point2>(p1, p2);
            return pair;
        }

        #endregion

        #region Events

        public event EventHandler<LineSweepEventArgs> Step;

        public void OnStep(LineSweepEventArgs e)
        {
            EventHandler<LineSweepEventArgs> handler = Step;
            if (handler != null) handler(this, e);
        }

        #endregion
    }
}
