using System.Collections.Generic;
using System.Linq;

namespace Json5
{
    /// <summary>
    /// Represents a JSON5 array.
    /// </summary>
    public class Json5Array : Json5Container, IList<Json5Value>
    {
        private readonly List<Json5Value> list = [];

        /// <summary>
        /// Gets the index of the specified item.
        /// </summary>
        /// <param name="item">The item to locate in the array.</param>
        /// <returns>The index of the item if found; otherwise, -1.</returns>
        public int IndexOf(Json5Value item)
        {
            return this.list.IndexOf(item);
        }

        /// <summary>
        /// Inserts an item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the item should be inserted.</param>
        /// <param name="item">The item to insert.</param>
        public void Insert(int index, Json5Value item)
        {
            this.list.Insert(index, item);
        }

        /// <summary>
        /// Removes the item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        public void RemoveAt(int index)
        {
            this.list.RemoveAt(index);
        }

        /// <summary>
        /// Gets or sets the item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to get or set.</param>
        /// <returns>The item at the specified index.</returns>
        public override Json5Value this[int index]
        {
            get { return this.list[index]; }
            set { this.list[index] = value; }
        }

        /// <summary>
        /// Adds an item to the array.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public void Add(Json5Value item)
        {
            this.list.Add(item);
        }

        /// <summary>
        /// Determines whether the array contains a specific value.
        /// </summary>
        /// <param name="item">The item to locate in the array.</param>
        /// <returns>true if the item is found; otherwise, false.</returns>
        public bool Contains(Json5Value item)
        {
            return this.list.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the array to an array, starting at a particular array index.
        /// </summary>
        /// <param name="array">The one-dimensional array that is the destination of the elements copied from the array.</param>
        /// <param name="arrayIndex">The zero-based index in the array at which copying begins.</param>
        void ICollection<Json5Value>.CopyTo(Json5Value[] array, int arrayIndex)
        {
            this.list.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets a value indicating whether the array is read-only.
        /// </summary>
        bool ICollection<Json5Value>.IsReadOnly
        {
            get { return ((ICollection<Json5Value>)this.list).IsReadOnly; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the array.
        /// </summary>
        /// <param name="item">The item to remove from the array.</param>
        /// <returns>true if the item was successfully removed; otherwise, false.</returns>
        public bool Remove(Json5Value item)
        {
            return this.list.Remove(item);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the array.
        /// </summary>
        /// <returns>An enumerator for the array.</returns>
        public IEnumerator<Json5Value> GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the array.
        /// </summary>
        /// <returns>An enumerator for the array.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Adds a value to the array at the specified key.
        /// </summary>
        /// <param name="key">The key at which to add the value.</param>
        /// <param name="value">The value to add.</param>
        public override void Add(string key, Json5Value value)
        {
            this[int.Parse(key)] = value;
        }

        /// <summary>
        /// Determines whether the array contains an element with the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the array.</param>
        /// <returns>true if the array contains an element with the key; otherwise, false.</returns>
        public override bool ContainsKey(string key)
        {
            var index = int.Parse(key);
            return index >= 0 && index < this.Count;
        }

        /// <summary>
        /// Gets a collection containing the keys in the array.
        /// </summary>
        public override ICollection<string> Keys
        {
            get { return new List<string>(Enumerable.Range(0, this.Count).Select(i => i.ToString())); }
        }

        /// <summary>
        /// Removes the element with the specified key from the array.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>true if the element is successfully removed; otherwise, false.</returns>
        public override bool Remove(string key)
        {
            this.RemoveAt(int.Parse(key));
            return true;
        }

        /// <summary>
        /// Gets a collection containing the values in the array.
        /// </summary>
        public override ICollection<Json5Value> Values
        {
            get { return new List<Json5Value>(this); }
        }

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get or set.</param>
        /// <returns>The value associated with the specified key.</returns>
        public override Json5Value this[string key]
        {
            get { return this[int.Parse(key)]; }
            set { this[int.Parse(key)] = value; }
        }

        /// <summary>
        /// Removes all items from the array.
        /// </summary>
        public override void Clear()
        {
            this.list.Clear();
        }

        /// <summary>
        /// Gets the number of elements contained in the array.
        /// </summary>
        public override int Count
        {
            get { return this.list.Count; }
        }

        /// <summary>
        /// Gets the type of the JSON5 value.
        /// </summary>
        public override Json5Type Type
        {
            get { return Json5Type.Array; }
        }

        /// <summary>
        /// Converts the array to a JSON5 string.
        /// </summary>
        /// <param name="space">The string to use for indentation.</param>
        /// <param name="indent">The current indentation level.</param>
        /// <param name="useOneSpaceIndent">Whether to use one space for indentation.</param>
        /// <returns>A JSON5 string representation of the array.</returns>
        internal override string ToJson5String(string space, string indent, bool useOneSpaceIndent = false)
        {
            // "If white space is used, trailing commas will be used in objects and arrays." from specification
            var forcedCommaAndNewLineRequired = !string.IsNullOrEmpty(space);

            var newLine = string.IsNullOrEmpty(space) ? "" : "\n";

            // TODO: Use string builder instead of string
            var s = indent + "[" + newLine;

            var isFirstValue = true;

            foreach (var value in this)
            {
                if (isFirstValue)
                {
                    isFirstValue = false;
                }
                else
                {
                    s += "," + newLine;
                }

                s += (value ?? Null).ToJson5String(space, indent + space);
            }

            if (forcedCommaAndNewLineRequired)
            {
                s += "," + newLine;
            }

            s += indent + "]";

            return s;
        }
    }
}
