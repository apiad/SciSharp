using System;
using System.Linq;


namespace SciSharp.Collections
{
    public static class DimensionStrategies
    {
        public static DimensionStrategy Cyclic(int length)
        {
            int curr = 0;
            return (vectors, left, right) => curr++%length;
        }

        public static int Longest(Vector[] vectors, int left, int right)
        {
            if (vectors == null) throw new ArgumentNullException("vectors");
            if (left < 0 || left >= vectors.Length) throw new ArgumentOutOfRangeException("left");
            if (right < 0 || right >= vectors.Length) throw new ArgumentOutOfRangeException("right");

            int size = vectors[0].Dimension;
            var min = new double[size];
            var max = new double[size];

            int count = left - right + 1;

            if (count%2 == 1)
            {
                for (int d = 0; d < size; d++)
                    min[d] = max[d] = vectors[left][d];

                left++;
            }

            for (int i = left; i < right; i += 2)
                for (int d = 0; d < size; d++)
                {
                    double dl = vectors[i][d];
                    double dr = vectors[i + 1][d];

                    if (dl > dr)
                    {
                        double temp = dr;
                        dr = dl;
                        dl = temp;
                    }

                    if (dl < min[i])
                        min[i] = dl;

                    if (dr > max[i])
                        max[i] = dr;
                }

            var deltas = new double[size];

            for (int d = 0; d < size; d++)
                deltas[d] = max[d] - min[d];

            return Enumerable.Range(0, size).ArgMax(i => deltas[i]);
        }
    }
}
