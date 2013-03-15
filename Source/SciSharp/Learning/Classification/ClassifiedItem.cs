namespace SciSharp.Learning.Classification
{
    /// <summary>
    /// Represents a tuple of item and class to be used
    /// in a classifier's training.
    /// </summary>
    /// <typeparam name="TItem">The type of the items to classify.</typeparam>
    /// <typeparam name="TClass">The type of the classes into which classify.</typeparam>
    public struct ClassifiedItem<TItem, TClass>
    {
        private readonly TItem item;
        private readonly TClass itemClass;

        /// <summary>
        /// Initializes a new instance of <see cref="ClassifiedItem{TItem,TClass}"/>.
        /// </summary>
        /// <param name="item">The item that has been classified.</param>
        /// <param name="itemClass">The class into which the item was classified.</param>
        public ClassifiedItem(TItem item, TClass itemClass)
        {
            this.item = item;
            this.itemClass = itemClass;
        }

        /// <summary>
        /// Gets the item that has been classified.
        /// </summary>
        public TItem Item
        {
            get { return item; }
        }

        /// <summary>
        /// Gets the class into which <see cref="Item"/> has been classified.
        /// </summary>
        public TClass Class
        {
            get { return itemClass; }
        }
    }
}
