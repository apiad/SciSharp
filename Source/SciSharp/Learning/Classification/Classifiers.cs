using System;
using System.Collections.Generic;


namespace SciSharp.Learning.Classification
{
    public static class Classifiers
    {
        public static IClassifier<TItem, TClass> FromMethod<TItem, TClass>(Func<TItem, TClass> classification)
        {
            if (classification == null)
                throw new ArgumentNullException("classification");

            return new MethodClassifier<TItem, TClass>(classification);
        }

        public static ClassifiedItem<TItem, TClass> ClassifyItem<TItem, TClass>(
            this IClassifier<TItem, TClass> classifier, TItem item)
        {
            return new ClassifiedItem<TItem, TClass>(item, classifier.Classify(item));
        }

        public static IEnumerable<ClassifiedItem<TItem, TClass>> ClassifyMany<TItem, TClass>(
            this IClassifier<TItem, TClass> classifier, IEnumerable<TItem> items)
        {
            foreach (TItem item in items)
                yield return classifier.ClassifyItem(item);
        }

        #region Nested type: MethodClassifier

        private class MethodClassifier<TItem, TClass> : IClassifier<TItem, TClass>
        {
            private readonly Func<TItem, TClass> classification;

            public MethodClassifier(Func<TItem, TClass> classification)
            {
                this.classification = classification;
            }

            #region IClassifier<TItem,TClass> Members

            public TClass Classify(TItem item)
            {
                return classification(item);
            }

            #endregion
        }

        #endregion
    }
}
