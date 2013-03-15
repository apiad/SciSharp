using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace SciSharp.Learning.Classification
{
    /// <summary>
    /// Represents an Adaboost classifier: a strong boolean
    /// classifier built from a sequence of weak boolean classifiers.
    /// It classifies as <value>true</value> if all classifiers return
    /// <value>true</value>, otherwise it classifies as <value>false</value>.
    /// </summary>
    /// <typeparam name="T">The type of items to classify.</typeparam>
    public class AdaboostClassifier<T> : IClassifier<T, bool>, IEnumerable<IClassifier<T, bool>>
    {
        private readonly List<IClassifier<T, bool>> classifiers;

        public AdaboostClassifier(params IClassifier<T, bool>[] classifiers)
        {
            if (classifiers == null)
                throw new ArgumentNullException("classifiers");

            foreach (var classifier in classifiers)
                if (classifier == null)
                    throw new ArgumentException("A classifier is null.");

            this.classifiers = new List<IClassifier<T, bool>>();
        }

        public AdaboostClassifier(IEnumerable<IClassifier<T, bool>> classifiers)
            : this(classifiers.ToArray()) {}

        public AdaboostClassifier()
        {
            classifiers = new List<IClassifier<T, bool>>();
        }

        #region IClassifier<T,bool> Members

        public bool Classify(T item)
        {
            foreach (var classifier in classifiers)
                if (!classifier.Classify(item))
                    return false;

            return true;
        }

        #endregion

        #region IEnumerable<IClassifier<T,bool>> Members

        public IEnumerator<IClassifier<T, bool>> GetEnumerator()
        {
            return classifiers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        public void Add(IClassifier<T, bool> classifier)
        {
            if (classifier == null)
                throw new ArgumentNullException("classifier");

            classifiers.Add(classifier);
        }
    }
}
