using System;

namespace Json5
{
    /// <summary>
    /// Exception thrown when a circular reference is detected during JSON5 conversion.
    /// </summary>
    public class Json5CircularReferenceException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Json5CircularReferenceException"/> class
        /// with a default error message indicating a circular structure.
        /// </summary>
        public Json5CircularReferenceException()
            : base("Converting circular structure to JSON5") { }
    }
}
