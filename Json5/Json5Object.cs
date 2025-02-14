using System.Collections.Generic;

namespace Json5
{
    using Parsing;

    /// <summary>
    /// Represents a JSON5 object.
    /// </summary>
    public class Json5Object : Json5Container, IEnumerable<KeyValuePair<string, Json5Value>>
    {
        private Dictionary<string, Json5Value> dictionary = new Dictionary<string, Json5Value>();

        /// <summary>
        /// Adds an element with the provided key and value to the object.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add.</param>
        public override void Add(string key, Json5Value value)
        {
            this.dictionary.Add(key, value);
        }

        /// <summary>
        /// Determines whether the object contains an element with the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the object.</param>
        /// <returns>true if the object contains an element with the key; otherwise, false.</returns>
        public override bool ContainsKey(string key)
        {
            return this.dictionary.ContainsKey(key);
        }

        /// <summary>
        /// Gets a collection containing the keys in the object.
        /// </summary>
        public override ICollection<string> Keys
        {
            get { return this.dictionary.Keys; }
        }

        /// <summary>
        /// Removes the element with the specified key from the object.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>true if the element is successfully removed; otherwise, false.</returns>
        public override bool Remove(string key)
        {
            return this.dictionary.Remove(key);
        }

        /// <summary>
        /// Gets a collection containing the values in the object.
        /// </summary>
        public override ICollection<Json5Value> Values
        {
            get { return this.dictionary.Values; }
        }

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get or set.</param>
        /// <returns>The value associated with the specified key.</returns>
        public override Json5Value this[string key]
        {
            get { return this.dictionary[key]; }
            set { this.dictionary[key] = value; }
        }

        /// <summary>
        /// Gets or sets the value at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the value to get or set.</param>
        /// <returns>The value at the specified index.</returns>
        public override Json5Value this[int index]
        {
            get { return this[index.ToString()]; }
            set { base[index.ToString()] = value; }
        }

        /// <summary>
        /// Removes all elements from the object.
        /// </summary>
        public override void Clear()
        {
            this.dictionary.Clear();
        }

        /// <summary>
        /// Gets the number of elements contained in the object.
        /// </summary>
        public override int Count
        {
            get { return this.dictionary.Count; }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the object.
        /// </summary>
        /// <returns>An enumerator for the object.</returns>
        public IEnumerator<KeyValuePair<string, Json5Value>> GetEnumerator()
        {
            return this.dictionary.GetEnumerator();
        }

        /// <summary>
        /// Gets the type of the JSON5 value.
        /// </summary>
        public override Json5Type Type
        {
            get { return Json5Type.Object; }
        }

        /// <summary>
        /// Converts the object to a JSON5 string.
        /// </summary>
        /// <param name="space">The string to use for indentation.</param>
        /// <param name="indent">The current indentation level.</param>
        /// <param name="useOneSpaceIndent">Whether to use one space for indentation.</param>
        /// <returns>A JSON5 string representation of the object.</returns>
        internal override string ToJson5String(string space, string indent, bool useOneSpaceIndent = false)
        {
            // "If white space is used, trailing commas will be used in objects and arrays." from specification
            bool forcedCommaAndNewLineRequired = !string.IsNullOrEmpty(space);

            string newLine = string.IsNullOrEmpty(space) ? "" : "\n";

            string currentIndent = useOneSpaceIndent ? " " : indent;

            // TODO: Use string builder instead of string
            string s = currentIndent + "{" + newLine;

            bool isFirstValue = true;

            foreach (var property in this)
            {
                if (isFirstValue)
                {
                    isFirstValue = false;
                }
                else
                {
                    s += "," + newLine;
                }

                s += indent + space + KeyToString(property.Key) + ":";

                s += (property.Value ?? Null).ToJson5String(space, indent + space, forcedCommaAndNewLineRequired);
            }

            if (forcedCommaAndNewLineRequired)
            {
                s += "," + newLine;
            }

            s += indent + "}";

            return s;
        }

        /// <summary>
        /// Converts a key to a JSON5 string representation.
        /// </summary>
        /// <param name="key">The key to convert.</param>
        /// <returns>A JSON5 string representation of the key.</returns>
        private string KeyToString(string key)
        {
            if (key.Length == 0)
                return "''";

            // This will not always work unless we check for Eof after the Identifier.
            // We should probably handle this another way.
            if (new Json5Lexer(key).Read().Type == Json5TokenType.Identifier)
                return key;

            return Json5.QuoteString(key);
        }
    }
}
