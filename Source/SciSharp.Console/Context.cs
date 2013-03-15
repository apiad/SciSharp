using System.Collections.Generic;


namespace SciSharp.Console
{
    public class Context
    {
        private readonly Dictionary<string, object> values;

        public Context()
        {
            values = new Dictionary<string, object>();
        }

        public T Get<T>(string key)
        {
            if (!values.ContainsKey(key))
                throw new KeyNotFoundException("Unknown variable: '{0}'".Formatted(key));

            return (T) values[key];
        }

        public void Set<T>(string key, T value)
        {
            values[key] = value;
        }
    }
}
