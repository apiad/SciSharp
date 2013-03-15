using System;
using System.Collections.Generic;


namespace SciSharp.Learning.Clustering
{
    public interface IClusterer<T>
    {
        IClusters<T> Run(IEnumerable<T> items, Func<T, T, double> distance);
    }
}
