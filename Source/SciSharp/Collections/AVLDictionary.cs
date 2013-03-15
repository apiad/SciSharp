using System;
using System.Collections;
using System.Collections.Generic;


namespace SciSharp.Collections
{
    public class AvlDictionary<TKey, TValue> : IDictionary<TKey, TValue>
        where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        private readonly AvlTree<KeyValuePair<TKey, TValue>> tree;

        public AvlDictionary()
        {
            Comparison<TKey> cmp = Comparer<TKey>.Default.Compare;
            tree = new AvlTree<KeyValuePair<TKey, TValue>>((x, y) => cmp(x.Key, y.Key));
        }

        public AvlDictionary(IComparer<TKey> comparer)
        {
            tree = new AvlTree<KeyValuePair<TKey, TValue>>((x, y) => comparer.Compare(x.Key, y.Key));
        }

        #region IDictionary<TKey,TValue> Members

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return tree.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            tree.Add(item);
        }

        public void Clear()
        {
            tree.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return tree.Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            tree.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return tree.Remove(item);
        }

        public int Count
        {
            get { return tree.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool ContainsKey(TKey key)
        {
            throw new NotImplementedException();
        }

        public void Add(TKey key, TValue value)
        {
            throw new NotImplementedException();
        }

        public bool Remove(TKey key)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            throw new NotImplementedException();
        }

        public TValue this[TKey key]
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public ICollection<TKey> Keys { get; private set; }
        public ICollection<TValue> Values { get; private set; }

        #endregion
    }
}
