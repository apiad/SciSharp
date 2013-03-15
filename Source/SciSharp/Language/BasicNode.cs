using System.Collections.Generic;


namespace SciSharp.Language
{
    /// <summary>
    /// Represents a node that can be used as a general-purpose attribute dictionary.
    /// Provides strong-typed methods for setting and getting attribute values.
    /// </summary>
    public class BasicNode : Node
    {
        /// <summary>
        /// Holds the internal attributes dictionary.
        /// </summary>
        private readonly Dictionary<string, object> properties;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicNode"/>.
        /// </summary>
        public BasicNode()
        {
            properties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Sets (overrides) the value of a given attribute.
        /// </summary>
        /// <typeparam name="T">Attribute type (generally inferred from usage).</typeparam>
        /// <param name="property">Attribute name.</param>
        /// <param name="value">Attribute value.</param>
        public void Set<T>(string property, T value)
        {
            properties[property] = value;
        }

        /// <summary>
        /// Gets the value of a given attribute casted as an specific type.
        /// </summary>
        /// <typeparam name="T">The type of the atribute.</typeparam>
        /// <param name="property">The name of the attribute.</param>
        /// <param name="def">The default value to give to the attribute if not found.</param>
        /// <returns>The value of the attribute casted to <typeparamref name="T"/>, or <paramref name="def"/>.</returns>
        public T Get<T>(string property, T def = default(T))
        {
            object val;
            return properties.TryGetValue(property, out val) ? (T) val : def;
        }
    }
}
