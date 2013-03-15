using System.Collections.Generic;


namespace SciSharp.Learning.Classification
{
    public class FeatureTrainer<T, TFeature, TClass, TClassifier> : ITrainer<T, TClass, TClassifier>
        where TClassifier : FeatureClassifier<T, TFeature, TClass>, new()
    {
        #region ITrainer<T,TClass,TClassifier> Members

        public TClassifier Train(IEnumerable<ClassifiedItem<T, TClass>> trainingSet)
        {
            var classifier = new TClassifier();

            foreach (var item in trainingSet)
                foreach (TFeature feature in classifier.GetFeatures(item.Item))
                    classifier.Train(feature, item.Class);

            return classifier;
        }

        #endregion
    }
}
