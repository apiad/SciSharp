using System.Collections.Generic;


namespace SciSharp.Learning.Classification
{
    public delegate IEnumerable<TFeature> FeatureExtraction<in T, out TFeature>(T item);
}
