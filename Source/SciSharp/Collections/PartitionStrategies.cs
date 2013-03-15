using System;


namespace SciSharp.Collections
{
    public static class PartitionStrategies
    {
        private static readonly Random Random = new Random();

        public static int Median(Vector[] vectors, int left, int right, int dimension)
        {
            if (vectors == null) throw new ArgumentNullException("vectors");
            if (left < 0 || left >= vectors.Length) throw new ArgumentOutOfRangeException("left");
            if (right < 0 || right >= vectors.Length) throw new ArgumentOutOfRangeException("right");

            return Select(vectors, left, right, (right - left + 1)/2, dimension);
        }

        public static PartitionStrategy RandomMedian(int samples)
        {
            var values = new double[samples];
            var idx = new int[samples];

            return (vectors, left, right, dimension) =>
                   {
                       for (int i = 0; i < idx.Length; i++)
                           idx[i] = i + left;

                       for (int i = 0; i < values.Length; i++)
                           values[i] = vectors[Random.Next(left, right + 1)][dimension];

                       Array.Sort(values, idx);
                       return idx[idx.Length/2];
                   };
        }

        private static int Select(Vector[] vectors, int left, int right, int i, int dimension)
        {
            if (left == right)
                return left;

            int pos = Partition(vectors, left, right, dimension);
            int k = pos - left + 1;

            if (i == k) return pos;
            if (i < k) return Select(vectors, left, pos - 1, i, dimension);
            return Select(vectors, pos + 1, right, i - k, dimension);
        }

        private static int Partition(Vector[] vectors, int left, int right, int dimension)
        {
            int r = Random.Next(left, right + 1);
            vectors.Swap(right, r);

            double x = vectors[right][dimension];
            int i = left - 1;

            for (int j = left; j < right; j++)
                if (vectors[j][dimension] <= x)
                {
                    i++;
                    vectors.Swap(i, j);
                }

            vectors.Swap(i + 1, right);
            return i + 1;
        }
    }
}
