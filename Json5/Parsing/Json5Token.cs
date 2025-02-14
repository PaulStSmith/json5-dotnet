using System.Diagnostics;

namespace Json5.Parsing
{
    /// <summary>
    /// Represents a JSON5 token.
    /// </summary>
    [DebuggerDisplay("Type = {Type}, Value = {Value}, Line = {Line}, Column = {Column}")]
    class Json5Token
    {
        /// <summary>
        /// Gets or sets the type of the token.
        /// </summary>
        public Json5TokenType Type { get; set; }

        /// <summary>
        /// Gets or sets the value of the token.
        /// </summary>
        public object Value { get; set; }

        private string input;

        /// <summary>
        /// Gets or sets the input string that generated the token.
        /// </summary>
        public string Input
        {
            get
            {
                if (this.input == null && this.Value != null)
                    return this.Value.ToString();

                return this.input;
            }

            set
            {
                this.input = value;
            }
        }

        /// <summary>
        /// Gets or sets the line number where the token was found.
        /// </summary>
        public int Line { get; set; }

        /// <summary>
        /// Gets or sets the column number where the token was found.
        /// </summary>
        public int Column { get; set; }

        /// <summary>
        /// Gets the string value of the token.
        /// </summary>
        public string String
        {
            get { return (string)this.Value; }
        }

        /// <summary>
        /// Gets the character value of the token.
        /// </summary>
        public char Character
        {
            get { return (char)this.Value; }
        }

        /// <summary>
        /// Gets the numeric value of the token.
        /// </summary>
        public double Number
        {
            get { return (double)this.Value; }
        }
    }
}
