using System;

namespace Json5
{
    /// <summary>
    /// Represents an exception that occurs during JSON5 parsing.
    /// </summary>
    public class Json5ParsingException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Json5ParsingException"/> class with a specified error message, line number, and column number.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="line">The line number where the error occurred.</param>
        /// <param name="column">The column number where the error occurred.</param>
        internal Json5ParsingException(string message, int line, int column) : base(message)
        {
            this.Line = line;
            this.Column = column;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Json5ParsingException"/> class with a specified inner exception, line number, and column number.
        /// </summary>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        /// <param name="line">The line number where the error occurred.</param>
        /// <param name="column">The column number where the error occurred.</param>
        internal Json5ParsingException(Exception innerException, int line, int column) : base(innerException.Message, innerException)
        {
            this.Line = line;
            this.Column = column;
        }

        /// <summary>
        /// Gets the line number where the error occurred.
        /// </summary>
        public int Line { get; }

        /// <summary>
        /// Gets the column number where the error occurred.
        /// </summary>
        public int Column { get; }
    }
}
