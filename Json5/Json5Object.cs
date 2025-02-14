using System.Collections.Generic;

namespace Json5
{
    using Parsing;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Represents a JSON5 object.
    /// </summary>
    public class Json5Object : Json5Container, IEnumerable<KeyValuePair<string, Json5Value>>
    {
        private readonly Dictionary<string, Json5Value> dictionary = [];

        // https://www.ecma-international.org/ecma-262/5.1/
        // Match IdentifierName (except escapes)
        private static readonly Regex identifierNameRegex = new(@"
            ^
                [\$_\p{L}\p{Nl}]
                [\$_\p{L}\p{Nl}\p{Mn}\p{Mc}\p{Nd}\p{Pc}\u200c\u200d]*
            $
        ", RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

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
            var forcedCommaAndNewLineRequired = !string.IsNullOrEmpty(space);
            var newLine = string.IsNullOrEmpty(space) ? "" : "\n";
            var currentIndent = useOneSpaceIndent ? " " : indent;

            var sb = new StringBuilder();
            sb.Append(currentIndent);
            sb.Append('{');
            sb.Append(newLine);

            var isFirstValue = true;

            foreach (var property in this)
            {
                if (isFirstValue)
                {
                    isFirstValue = false;
                }
                else
                {
                    sb.Append(',');
                    sb.Append(newLine);
                }

                sb.Append(indent);
                sb.Append(space);
                sb.Append(KeyToString(property.Key));
                sb.Append(':');
                sb.Append((property.Value ?? Null).ToJson5String(space, indent + space, forcedCommaAndNewLineRequired));
            }

            if (forcedCommaAndNewLineRequired)
            {
                sb.Append(',').Append(newLine);
            }

            sb.Append(indent).Append('}');

            return sb.ToString();
        }

        /// <summary>
        /// Converts a key to a JSON5 string representation.
        /// </summary>
        /// <param name="key">The key to convert.</param>
        /// <returns>A JSON5 string representation of the key.</returns>
        private string KeyToString(string key)
        {
            if (identifierNameRegex.IsMatch(key))
                return key;

            return Json5.QuoteString(key);
        }
    }
}
