using System;
using System.Collections.Generic;
using System.Linq;

using SciSharp.Probabilities;


namespace SciSharp.Learning.Clustering
{
    public class KMeansClusterer<T> : IFixedClusterer<T>
    {
        public int IterationsPerCluster { get; set; }

        #region IFixedClusterer<T> Members

        public IClusters<T> Run(IEnumerable<T> items, Func<T, T, double> distance, int clusters)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            if (distance == null)
                throw new ArgumentNullException("distance");

            if (clusters <= 0)
                throw new ArgumentOutOfRangeException("clusters", "Must be greater than zero.");

            T[] allItems = items.ToArray();

            if (clusters > allItems.Length)
                throw new ArgumentOutOfRangeException("clusters", "Must be less or equal that the number of items.");

            T[] centroids = RandomEx.Instance.Sample(allItems, clusters);
            var results = new KMeansClusters<T>(centroids, distance);

            for (int i = 0; i < IterationsPerCluster*clusters; i++) {}

            throw new NotImplementedException();
        }

        #endregion
    }
}
