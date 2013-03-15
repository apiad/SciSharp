using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;


namespace SciSharp.Collections
{
    public partial class KdTree
    {
        private readonly float epsilon;
        private readonly KdNode root;

        protected KdTree(KdNode root, float epsilon)
        {
            this.root = root;
            this.epsilon = epsilon;
        }

        public static bool ParallelOptimizations { get; set; }

        public int Count
        {
            get { return root != null ? root.Count : 0; }
        }

        public int Depth
        {
            get { return root != null ? root.Depth : 0; }
        }

        public static KdTree Build(Vector[] vectors, PartitionStrategy partitionStrategy, DimensionStrategy pickingStrategy, float epsilon = float.MaxValue)
        {
            if (vectors == null)
                throw new ArgumentNullException("vectors");
            if (partitionStrategy == null)
                throw new ArgumentNullException("partitionStrategy");
            if (pickingStrategy == null)
                throw new ArgumentNullException("pickingStrategy");

            var maxParallelDepth = (int) Math.Log(Environment.ProcessorCount, 2);
            var result = new KdTree(Build(vectors, partitionStrategy, pickingStrategy, 0, vectors.Length - 1, 0, maxParallelDepth), epsilon);
            return result;
        }

        private static KdNode Build(Vector[] vectors, PartitionStrategy partition, DimensionStrategy picking, int left, int right, int depth, int maxParallelDepth)
        {
            if (left > right)
                return null;

            int dim = picking(vectors, left, right);
            int vec = partition(vectors, left, right, dim);

            if (ParallelOptimizations && depth < maxParallelDepth)
            {
                KdNode leftNode = null, rightNode = null;
                var leftThread = new Thread(() => leftNode = Build(vectors, partition, picking, left, vec - 1, depth + 1, maxParallelDepth));
                var rightThread = new Thread(() => rightNode = Build(vectors, partition, picking, vec + 1, right, depth + 1, maxParallelDepth));

                leftThread.Start();
                rightThread.Start();

                leftThread.Join();
                rightThread.Join();

                return new KdNode(vectors[vec][dim], dim, leftNode, rightNode, vectors[vec]);
            }

            KdNode leftNodeSync = Build(vectors, partition, picking, left, vec - 1, depth, maxParallelDepth);
            KdNode rightNodeSync = Build(vectors, partition, picking, vec + 1, right, depth, maxParallelDepth);

            return new KdNode(vectors[vec][dim], dim, leftNodeSync, rightNodeSync, vectors[vec]);
        }

        public override string ToString()
        {
            if (root != null)
                return root.ToString();

            return string.Empty;
        }

        public IEnumerable<Vector> Closest(Vector vector, float maxDist = float.MaxValue)
        {
            return Closest(1, vector, maxDist);
        }

        public IEnumerable<Vector> Closest(int count, Vector vector, float maxDist = float.MaxValue)
        {
            return root == null ? Enumerable.Empty<Vector>() : root.Closest(count, vector, maxDist);
        }
    }
}
