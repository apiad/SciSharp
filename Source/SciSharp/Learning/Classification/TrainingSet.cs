using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace SciSharp.Learning.Classification
{
    public class TrainingSet<T, TClass> : IEnumerable<ClassifiedItem<T, TClass>>
    {
        private List<ClassifiedItem<T, TClass>> items;

        public TrainingSet(IEnumerable<ClassifiedItem<T, TClass>> set)
        {
            items = new List<ClassifiedItem<T, TClass>>(set);
        }

        public TrainingSet()
        {
            items = new List<ClassifiedItem<T, TClass>>();
        }

        #region IEnumerable<ClassifiedItem<T,TClass>> Members

        public IEnumerator<ClassifiedItem<T, TClass>> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        public void Add(ClassifiedItem<T, TClass> item)
        {
            items.Add(item);
        }

        public void Add(T item, TClass itemClass)
        {
            items.Add(new ClassifiedItem<T, TClass>(item, itemClass));
        }

        public TrainingSet<T, TClass> Partition(int count)
        {
            var set = new TrainingSet<T, TClass>(items.Take(count));
            items = new List<ClassifiedItem<T, TClass>>(items.Skip(count));
            return set;
        }

        public TrainingResult<TClass> Test(IClassifier<T, TClass> classifier)
        {
            var result = new TrainingResult<TClass>();

            foreach (var classifiedItem in items)
                result.Add(classifiedItem.Class, classifier.Classify(classifiedItem.Item));

            return result;
        }
    }
}
