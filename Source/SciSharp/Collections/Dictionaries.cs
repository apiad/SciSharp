using System.Collections.Generic;


namespace SciSharp.Collections
{
    public static class Dictionaries
    {
        public static TValue Get<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key,
                                               TValue defaultValue = default(TValue), bool add = false)
        {
            TValue value;

            if (!dictionary.TryGetValue(key, out value))
            {
                value = defaultValue;

                if (add)
                    dictionary[key] = value;
            }

            return value;
        }
    }
}
