using System.Collections.Generic;


namespace SciSharp.Learning.Classification
{
    /// <summary>
    /// Represents a classifier factory that takes a training
    /// set and produces a ready-to-use classifer.
    /// </summary>
    /// <typeparam name="T">The type of the items to classify.</typeparam>
    /// <typeparam name="TClass">The type of the classes into which classify.</typeparam>
    /// <typeparam name="TClassifier">The type of the classifier builder.</typeparam>
    public interface ITrainer<T, TClass, out TClassifier>
        where TClassifier : IClassifier<T, TClass>
    {
        /// <summary>
        /// Builds a classifier out of a training set.
        /// </summary>
        /// <param name="trainingSet">A set of correctly classified items used to build and train the classifier.</param>
        /// <returns>An instance of <typeparamref name="TClassifier"/> trained with the specified set.</returns>
        TClassifier Train(IEnumerable<ClassifiedItem<T, TClass>> trainingSet);
    }
}
