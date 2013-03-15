using System;

using SciSharp.Learning.Classification;


namespace SciSharp.Learning.Clustering
{
    public class KMeansClusters<T> : IClusters<T>, IClassifier<T, int>
    {
        private readonly T[] centroids;
        private readonly Func<T, T, double> distance;

        public KMeansClusters(T[] centroids, Func<T, T, double> distance)
        {
            this.centroids = centroids;
            this.distance = distance;
        }

        #region IClassifier<T,int> Members

        public int Classify(T item)
        {
            int min = 0;
            double d = distance(item, centroids[min]);

            for (int i = 1; i < Clusters; i++)
            {
                double newD = distance(item, centroids[i]);

                if (newD < d)
                {
                    min = i;
                    d = newD;
                }
            }

            return min;
        }

        #endregion

        #region IClusters<T> Members

        public int ClusterOf(T item)
        {
            return Classify(item);
        }

        public int Clusters
        {
            get { return centroids.Length; }
        }

        #endregion
    }
}
