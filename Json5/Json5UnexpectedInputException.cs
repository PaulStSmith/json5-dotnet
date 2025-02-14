namespace Json5
{
    /// <summary>
    /// Represents an exception that is thrown when unexpected input is encountered during JSON5 parsing.
    /// </summary>
    public class Json5UnexpectedInputException : Json5ParsingException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Json5UnexpectedInputException"/> class with the specified input, line, and column numbers.
        /// </summary>
        /// <param name="input">The unexpected input.</param>
        /// <param name="line">The line number where the unexpected input was encountered.</param>
        /// <param name="column">The column number where the unexpected input was encountered.</param>
        internal Json5UnexpectedInputException(string input, int line, int column) :
          base(string.Format("Unexpected input '{0}' at line {1} column {2}", input, line, column), line, column)
        { }
    }
}
