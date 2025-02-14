using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Json5
{
    /// <summary>  
    /// Represents a JSON5 container that can hold key-value pairs.  
    /// </summary>  
    public abstract class Json5Container : Json5Value, IDictionary<string, Json5Value>
    {
        /// <summary>  
        /// Adds an element with the provided key and value to the container.  
        /// </summary>  
        /// <param name="key">The key of the element to add.</param>  
        /// <param name="value">The value of the element to add.</param>  
        public abstract void Add(string key, Json5Value value);

        /// <summary>  
        /// Determines whether the container contains an element with the specified key.  
        /// </summary>  
        /// <param name="key">The key to locate in the container.</param>  
        /// <returns>true if the container contains an element with the key; otherwise, false.</returns>  
        public abstract bool ContainsKey(string key);

        /// <summary>  
        /// Gets a collection containing the keys in the container.  
        /// </summary>  
        public abstract ICollection<string> Keys { get; }

        /// <summary>  
        /// Removes the element with the specified key from the container.  
        /// </summary>  
        /// <param name="key">The key of the element to remove.</param>  
        /// <returns>true if the element is successfully removed; otherwise, false.</returns>  
        public abstract bool Remove(string key);

        /// <summary>  
        /// Tries to get the value associated with the specified key.  
        /// </summary>  
        /// <param name="key">The key of the value to get.</param>  
        /// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, null.</param>  
        /// <returns>true if the container contains an element with the key; otherwise, false.</returns>  
        bool IDictionary<string, Json5Value>.TryGetValue(string key, out Json5Value value)
        {
            value = null;
            if (!this.ContainsKey(key))
                return false;

            value = this[key];
            return true;
        }

        /// <summary>  
        /// Gets a collection containing the values in the container.  
        /// </summary>  
        public abstract ICollection<Json5Value> Values { get; }

        /// <summary>  
        /// Adds an item to the container.  
        /// </summary>  
        /// <param name="item">The item to add.</param>  
        void ICollection<KeyValuePair<string, Json5Value>>.Add(KeyValuePair<string, Json5Value> item)
        {
            this.Add(item.Key, item.Value);
        }

        /// <summary>  
        /// Removes all items from the container.  
        /// </summary>  
        public abstract void Clear();

        /// <summary>  
        /// Determines whether the container contains a specific key-value pair.  
        /// </summary>  
        /// <param name="item">The key-value pair to locate in the container.</param>  
        /// <returns>true if the container contains the key-value pair; otherwise, false.</returns>  
        bool ICollection<KeyValuePair<string, Json5Value>>.Contains(KeyValuePair<string, Json5Value> item)
        {
            return this.ContainsKey(item.Key) && this[item.Key] == item.Value;
        }

        /// <summary>  
        /// Copies the elements of the container to an array, starting at a particular array index.  
        /// </summary>  
        /// <param name="array">The one-dimensional array that is the destination of the elements copied from the container.</param>  
        /// <param name="arrayIndex">The zero-based index in the array at which copying begins.</param>  
        void ICollection<KeyValuePair<string, Json5Value>>.CopyTo(KeyValuePair<string, Json5Value>[] array, int arrayIndex)
        {
            this.ToArray().CopyTo(array, arrayIndex);
        }

        /// <summary>  
        /// Gets the number of elements contained in the container.  
        /// </summary>  
        public abstract int Count { get; }

        /// <summary>  
        /// Gets a value indicating whether the container is read-only.  
        /// </summary>  
        bool ICollection<KeyValuePair<string, Json5Value>>.IsReadOnly
        {
            get { return false; }
        }

        /// <summary>  
        /// Removes the first occurrence of a specific key-value pair from the container.  
        /// </summary>  
        /// <param name="item">The key-value pair to remove from the container.</param>  
        /// <returns>true if the key-value pair was successfully removed; otherwise, false.</returns>  
        bool ICollection<KeyValuePair<string, Json5Value>>.Remove(KeyValuePair<string, Json5Value> item)
        {
            if (((ICollection<KeyValuePair<string, Json5Value>>)this).Contains(item))
                return this.Remove(item.Key);

            return false;
        }

        /// <summary>  
        /// Returns an enumerator that iterates through the container.  
        /// </summary>  
        /// <returns>An enumerator for the container.</returns>  
        IEnumerator<KeyValuePair<string, Json5Value>> IEnumerable<KeyValuePair<string, Json5Value>>.GetEnumerator()
        {
            foreach (var key in this.Keys)
                yield return new KeyValuePair<string, Json5Value>(key, this[key]);
        }

        /// <summary>  
        /// Returns an enumerator that iterates through the container.  
        /// </summary>  
        /// <returns>An enumerator for the container.</returns>  
        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var key in this.Keys)
                yield return new KeyValuePair<string, Json5Value>(key, this[key]);
        }

        /// <summary>
        /// Converts the container to a JSON5 string representation.
        /// </summary>
        /// <param name="space">The string to use for indentation.</param>
        /// <param name="indent">The current indentation level.</param>
        /// <param name="useOneSpaceIndent">Whether to use one space for indentation.</param>
        /// <param name="tracker">The reference tracker to detect circular references.</param>
        /// <returns>A JSON5 string representation of the container.</returns>
        internal abstract string ToJson5String(string space, string indent, bool useOneSpaceIndent, ReferenceTracker tracker);

        /// <summary>
        /// Converts the container to a JSON5 string representation.
        /// </summary>
        /// <param name="space">The string to use for indentation.</param>
        /// <param name="indent">The current indentation level.</param>
        /// <param name="useOneSpaceIndent">Whether to use one space for indentation.</param>
        /// <returns>A JSON5 string representation of the container.</returns>
        internal override string ToJson5String(string space, string indent = "", bool useOneSpaceIndent = false)
        {
            var tracker = new ReferenceTracker();
            return ToJson5String(space, indent, useOneSpaceIndent, tracker);
        }
    }
}
