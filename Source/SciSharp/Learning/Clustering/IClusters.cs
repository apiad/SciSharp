namespace SciSharp.Learning.Clustering
{
    public interface IClusters<T>
    {
        int Clusters { get; }

        int ClusterOf(T item);
    }
}
