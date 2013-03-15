using System.Collections.Generic;
using System.Linq;

using SciSharp.Collections;


namespace SciSharp.Learning.Classification
{
    /// <summary>
    /// Represents an abstract classifier which is based on
    /// extracting a set of features from an item and classifying it
    /// according to the previous appeareance of such features
    /// on the training set.
    /// </summary>
    /// <typeparam name="T">The type of the items to be classified.</typeparam>
    /// <typeparam name="TFeature">The type of the features extracted.</typeparam>
    /// <typeparam name="TClass">The type of the classes into which classify.</typeparam>
    public abstract class FeatureClassifier<T, TFeature, TClass> : IClassifier<T, TClass>
    {
        private readonly DefaultDictionary<TClass, DefaultDictionary<TFeature, int>> classes;
        private readonly DefaultDictionary<TFeature, int> featuresCount;

        protected FeatureClassifier()
        {
            classes = new DefaultDictionary<TClass, DefaultDictionary<TFeature, int>>();
            featuresCount = new DefaultDictionary<TFeature, int>();
        }

        /// <summary>
        /// Gets the count of classes that have been seen at least once
        /// in the training set.
        /// </summary>
        public int Classes
        {
            get { return classes.Keys.Count; }
        }

        /// <summary>
        /// Gets the number of different features seen.
        /// </summary>
        public int Features
        {
            get { return featuresCount.Count; }
        }

        #region IClassifier<T,TClass> Members

        public virtual TClass Classify(T item)
        {
            IEnumerable<TFeature> features = GetFeatures(item);

            var classValues = from itemClass in classes.Keys
                              let classFeatures = from feature in features
                                                  select new FeatureCount<TFeature>(feature, Count(itemClass, feature))
                              select new {Class = itemClass, Evaluation = Evaluate(itemClass, classFeatures)};

            return classValues.ArgMax(value => value.Evaluation).Class;
        }

        #endregion

        /// <summary>
        /// Gets the number of times a given feature has been
        /// seen in an specific class in the training set.
        /// </summary>
        /// <param name="featureClass">The class to count.</param>
        /// <param name="feature">The observed feature.</param>
        /// <returns>An integer indicating how many times the given
        /// feature was seen in the given class in the training set.</returns>
        public int Count(TClass featureClass, TFeature feature)
        {
            DefaultDictionary<TFeature, int> featureCount;

            if (!classes.TryGetValue(featureClass, out featureCount))
                return 0;

            return featureCount[feature];
        }

        /// <summary>
        /// Gets the total number of times an specific 
        /// feature has been seen among all classes.
        /// </summary>
        /// <param name="feature">The observed feature.</param>
        /// <returns>An integer indicating the number of
        /// times the given feature has been seen in the
        /// training set.</returns>
        public int FeatureCount(TFeature feature)
        {
            return featuresCount[feature];
        }

        /// <summary>
        /// Adds a feature to the training set specifying which
        /// class the feature's owner item belongs to.
        /// </summary>
        /// <param name="feature">The observed feature.</param>
        /// <param name="itemClass">The class to which the feature's owner item belongs to.</param>
        public void Train(TFeature feature, TClass itemClass)
        {
            DefaultDictionary<TFeature, int> featureCount;

            if (!classes.TryGetValue(itemClass, out featureCount))
                featureCount = classes[itemClass] = new DefaultDictionary<TFeature, int>();

            featureCount[feature]++;
            featuresCount[feature]++;
        }

        protected abstract double Evaluate(TClass itemClass, IEnumerable<FeatureCount<TFeature>> classFeatures);

        /// <summary>
        /// Extracts a set of features from a given item.
        /// </summary>
        /// <param name="item">The item to process.</param>
        /// <returns>An enumerable of features that can be extracted from the given item.</returns>
        public abstract IEnumerable<TFeature> GetFeatures(T item);
    }
}
