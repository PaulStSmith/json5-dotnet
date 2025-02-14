using Json5;
using System;
using System.Collections.Generic;

namespace Json5
{
    /// <summary>
    /// Tracks references to JSON5 values to detect circular references.
    /// </summary>
    internal class ReferenceTracker
    {
        private readonly HashSet<Json5Value> references = [];

        /// <summary>
        /// Adds a JSON5 value to the reference tracker.
        /// Throws <see cref="Json5CircularReferenceException"/> if a circular reference is detected.
        /// </summary>
        /// <param name="value">The JSON5 value to add.</param>
        public void Push(Json5Value value)
        {
            if (value is Json5Container container)
            {
                if (!references.Add(container))
                    throw new Json5CircularReferenceException();
            }
        }

        /// <summary>
        /// Removes a JSON5 value from the reference tracker.
        /// </summary>
        /// <param name="value">The JSON5 value to remove.</param>
        public void Pop(Json5Value value)
        {
            if (value is Json5Container container)
            {
                references.Remove(container);
            }
        }
    }
}
