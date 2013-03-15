using System;
using System.Collections;
using System.Collections.Generic;


namespace SciSharp.Collections
{
    public class DefaultDictionary<TKey, TValue> : IDictionary<TKey, TValue>
        where TValue : new()
    {
        private readonly Func<TKey, TValue> factory;
        private readonly IDictionary<TKey, TValue> internalDictionary;

        public DefaultDictionary(Func<TKey, TValue> factory)
            : this(new Dictionary<TKey, TValue>(), factory)
        {
            this.factory = factory;
        }

        public DefaultDictionary() :
            this(key => new TValue())
        {
            AddOnGet = true;
        }

        public DefaultDictionary(IDictionary<TKey, TValue> internalDictionary, Func<TKey, TValue> factory)
        {
            if (internalDictionary == null)
                throw new ArgumentNullException("internalDictionary");

            this.internalDictionary = internalDictionary;
            this.factory = factory;
        }

        public bool AddOnGet { get; set; }

        #region IDictionary<TKey,TValue> Members

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return internalDictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            internalDictionary.Add(item);
        }

        public void Clear()
        {
            internalDictionary.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return internalDictionary.Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            internalDictionary.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return internalDictionary.Remove(item);
        }

        public int Count
        {
            get { return internalDictionary.Count; }
        }

        public bool IsReadOnly
        {
            get { return internalDictionary.IsReadOnly; }
        }

        public bool ContainsKey(TKey key)
        {
            return internalDictionary.ContainsKey(key);
        }

        public void Add(TKey key, TValue value)
        {
            internalDictionary.Add(key, value);
        }

        public bool Remove(TKey key)
        {
            return internalDictionary.Remove(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return internalDictionary.TryGetValue(key, out value);
        }

        public TValue this[TKey key]
        {
            get
            {
                TValue result = this.Get(key, factory(key));

                if (AddOnGet)
                    internalDictionary[key] = result;

                return result;
            }
            set { internalDictionary[key] = value; }
        }

        public ICollection<TKey> Keys
        {
            get { return internalDictionary.Keys; }
        }

        public ICollection<TValue> Values
        {
            get { return internalDictionary.Values; }
        }

        #endregion

        public T Internal<T>()
            where T : IDictionary<TKey, TValue>
        {
            return (T) internalDictionary;
        }
    }
}
