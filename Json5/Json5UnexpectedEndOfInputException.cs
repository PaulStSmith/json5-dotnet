namespace Json5
{
    /// <summary>
    /// Represents an exception that is thrown when the end of input is unexpectedly reached during JSON5 parsing.
    /// </summary>
    public class Json5UnexpectedEndOfInputException : Json5ParsingException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Json5UnexpectedEndOfInputException"/> class with the specified line and column numbers.
        /// </summary>
        /// <param name="line">The line number where the end of input was reached.</param>
        /// <param name="column">The column number where the end of input was reached.</param>
        internal Json5UnexpectedEndOfInputException(int line, int column) :
          base(string.Format("Unexpected end of input at line {0} column {1}", line, column), line, column)
        { }
    }
}
