using System.Collections.Generic;


namespace SciSharp.Learning.Classification
{
    /// <summary>
    /// Implements a feature classifier based on the Näive-Bayes technique.
    /// </summary>
    /// <typeparam name="T">The type of the items to be classified.</typeparam>
    /// <typeparam name="TFeature">The type of the features extracted.</typeparam>
    /// <typeparam name="TClass">The type of the classes into which classify.</typeparam>
    public class NaiveBayesClassifier<T, TFeature, TClass> : FeatureClassifier<T, TFeature, TClass>
    {
        private readonly FeatureExtraction<T, TFeature> featureExtraction;

        public NaiveBayesClassifier(FeatureExtraction<T, TFeature> featureExtraction)
        {
            this.featureExtraction = featureExtraction;
        }

        protected override double Evaluate(TClass itemClass, IEnumerable<FeatureCount<TFeature>> classFeatures)
        {
            double p = 1;

            foreach (var featureCount in classFeatures)
                p *= (featureCount.Count + 1)/(double) (FeatureCount(featureCount.Feature) + 1);

            return p;
        }

        public override IEnumerable<TFeature> GetFeatures(T item)
        {
            return featureExtraction(item);
        }
    }
}
