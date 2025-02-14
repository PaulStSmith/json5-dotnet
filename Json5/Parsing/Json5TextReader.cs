using System.IO;

namespace Json5.Parsing
{
    /// <summary>
    /// Reads JSON5 text from a <see cref="TextReader"/> and keeps track of line and column numbers.
    /// </summary>
    class Json5TextReader
    {
        private readonly TextReader reader;

        /// <summary>
        /// Initializes a new instance of the <see cref="Json5TextReader"/> class with the specified <see cref="TextReader"/>.
        /// </summary>
        /// <param name="reader">The <see cref="TextReader"/> to read JSON5 text from.</param>
        public Json5TextReader(TextReader reader)
        {
            this.reader = reader;
        }

        /// <summary>
        /// Gets the current line number.
        /// </summary>
        public int Line { get; private set; } = 1;

        /// <summary>
        /// Gets the current column number.
        /// </summary>
        public int Column { get; private set; } = 1;

        /// <summary>
        /// Reads the next character from the input text and advances the line and column numbers as appropriate.
        /// </summary>
        /// <returns>The next character from the input text, or -1 if no more characters are available.</returns>
        public int Read()
        {
            var r = this.reader.Read();

            if (r == '\n')
            {
                this.Line++;
                this.Column = 1;
            }
            else if (r >= 0)
                this.Column++;

            return r;
        }

        /// <summary>
        /// Returns the next available character without advancing the reader.
        /// </summary>
        /// <returns>The next available character, or -1 if no more characters are available.</returns>
        public int Peek()
        {
            return this.reader.Peek();
        }
    }
}
