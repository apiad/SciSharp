using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace SciSharp.Learning.Classification
{
    /// <summary>
    /// Represents a commitee of classifiers, which
    /// classifies an item based on a weighted classification
    /// of a set of classifiers.
    /// </summary>
    /// <typeparam name="T">The type of the items to classify.</typeparam>
    /// <typeparam name="TClass">The type of the classes into which classify.</typeparam>
    public class ClassifiersCommitee<T, TClass> : IClassifier<T, TClass>, IEnumerable<KeyValuePair<IClassifier<T, TClass>, double>>, IEnumerable<IClassifier<T, TClass>>
    {
        private readonly IDictionary<IClassifier<T, TClass>, double> classifiers;

        public ClassifiersCommitee(IDictionary<IClassifier<T, TClass>, double> classifiers)
        {
            if (classifiers == null)
                throw new ArgumentNullException("classifiers");

            this.classifiers = classifiers;
        }

        public ClassifiersCommitee()
        {
            classifiers = new Dictionary<IClassifier<T, TClass>, double>();
        }

        #region IClassifier<T,TClass> Members

        public TClass Classify(T item)
        {
            var classes = classifiers
                .Select(classifier => new {Class = classifier.Key.Classify(item), Weight = classifier.Value})
                .GroupBy(data => data.Class, data => data.Weight, (cls, items) => new {Class = cls, Weight = items.Sum()});

            return classes.ArgMax(data => data.Weight).Class;
        }

        #endregion

        #region IEnumerable<IClassifier<T,TClass>> Members

        public IEnumerator<IClassifier<T, TClass>> GetEnumerator()
        {
            return classifiers.Keys.GetEnumerator();
        }

        #endregion

        #region IEnumerable<KeyValuePair<IClassifier<T,TClass>,double>> Members

        IEnumerator<KeyValuePair<IClassifier<T, TClass>, double>> IEnumerable<KeyValuePair<IClassifier<T, TClass>, double>>.GetEnumerator()
        {
            return classifiers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        public void Add(IClassifier<T, TClass> classifier)
        {
            Add(classifier, 1d);
        }

        public void Add(IClassifier<T, TClass> classifier, double weight)
        {
            classifiers[classifier] = weight;
        }
    }
}
