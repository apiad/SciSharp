using System;
using System.Collections.Generic;


namespace SciSharp.Learning.Clustering
{
    public class ClustersBase<T> : IClusters<T>
    {
        private readonly Dictionary<T, int> clusters;
        private readonly int count;

        public ClustersBase(IEnumerable<T> items, Func<T, int> clustering)
        {
            clusters = new Dictionary<T, int>();

            int max = 0;

            foreach (T item in items)
            {
                int cluster = clustering(item);
                clusters[item] = cluster;
                max = Math.Max(max, cluster);
            }

            count = max + 1;
        }

        #region IClusters<T> Members

        public int ClusterOf(T item)
        {
            return clusters[item];
        }

        public int Clusters
        {
            get { return count; }
        }

        #endregion
    }
}
