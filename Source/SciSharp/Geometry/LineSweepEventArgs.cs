using System;


namespace SciSharp.Geometry
{
    public class LineSweepEventArgs : EventArgs
    {
        public Pair<Point2> BestSoFar { get; set; }
        public Pair<Point2> CurrentPair { get; set; }
        public Pair<Point2>? LeftMin { get; set; }
        public Pair<Point2>? RightMin { get; set; }
        public double XMin { get; set; }
        public double YMin { get; set; }
        public double XMax { get; set; }
        public double YMax { get; set; }
        public Point2[] Points { get; set; }

        public LineSweepEventArgs(Pair<Point2> bestSoFar, double xMin, double yMin, double xMax, double yMax, Point2[] points, Pair<Point2>? rightMin, Pair<Point2>? leftMin, Pair<Point2> currentPair)
        {
            BestSoFar = bestSoFar;
            XMin = xMin;
            YMin = yMin;
            XMax = xMax;
            YMax = yMax;
            Points = points;
            RightMin = rightMin;
            LeftMin = leftMin;
            CurrentPair = currentPair;
        }
    }
}