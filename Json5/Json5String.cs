namespace Json5
{
    /// <summary>
    /// Represents a JSON5 string value.
    /// </summary>
    public class Json5String : Json5Primitive
    {
        private readonly string value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Json5String"/> class with the specified value.
        /// </summary>
        /// <param name="value">The string value.</param>
        public Json5String(string value)
        {
            this.value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Json5String"/> class with the specified character array.
        /// </summary>
        /// <param name="value">The character array.</param>
        public Json5String(char[] value) : this(new string(value)) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Json5String"/> class with the specified object.
        /// </summary>
        /// <param name="value">The object value.</param>
        public Json5String(object value) : this(value.ToString()) { }

        /// <summary>
        /// Gets the type of the JSON5 value.
        /// </summary>
        public override Json5Type Type
        {
            get { return Json5Type.String; }
        }

        /// <summary>
        /// Gets the string value.
        /// </summary>
        protected override object Value
        {
            get { return this.value; }
        }

        /// <summary>
        /// Converts the string value to a JSON5 string.
        /// </summary>
        /// <param name="space">The string to use for indentation.</param>
        /// <param name="indent">The current indentation level.</param>
        /// <param name="useOneSpaceIndent">Whether to use one space for indentation.</param>
        /// <returns>A JSON5 string representation of the string value.</returns>
        internal override string ToJson5String(string space, string indent, bool useOneSpaceIndent = false)
        {
            return AddIndent(Json5.QuoteString(this.value), indent, useOneSpaceIndent);
        }

        /// <summary>
        /// Defines an implicit conversion of a <see cref="Json5String"/> to a <see cref="string"/>.
        /// </summary>
        /// <param name="value">The <see cref="Json5String"/> to convert.</param>
        public static implicit operator string(Json5String value)
        {
            return value.value;
        }
    }
}
