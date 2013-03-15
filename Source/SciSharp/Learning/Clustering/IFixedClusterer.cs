using System;
using System.Collections.Generic;


namespace SciSharp.Learning.Clustering
{
    public interface IFixedClusterer<T>
    {
        IClusters<T> Run(IEnumerable<T> items, Func<T, T, double> distance, int clusters);
    }
}
