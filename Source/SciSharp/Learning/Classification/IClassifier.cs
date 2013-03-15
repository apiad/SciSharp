namespace SciSharp.Learning.Classification
{
    /// <summary>
    /// Represents a classifier of a set of items
    /// into a set of classes.
    /// </summary>
    /// <typeparam name="T">The type of the items to classify.</typeparam>
    /// <typeparam name="TClass">The type of the classes into which classify.</typeparam>
    public interface IClassifier<in T, out TClass>
    {
        /// <summary>
        /// Computes the most likely class for a given
        /// item based on the classifiers internal state.
        /// </summary>
        /// <param name="item">The item to classify.</param>
        /// <returns>An instance of <typeparamref name="TClass"/> indicating the
        /// most likely class the specified item belongs to.</returns>
        TClass Classify(T item);
    }
}
