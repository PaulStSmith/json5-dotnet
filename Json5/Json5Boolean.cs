namespace Json5
{
    /// <summary>
    /// Represents a JSON5 boolean value.
    /// </summary>
    public class Json5Boolean : Json5Primitive
    {
        private readonly bool value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Json5Boolean"/> class with the specified value.
        /// </summary>
        /// <param name="value">The boolean value.</param>
        public Json5Boolean(bool value)
        {
            this.value = value;
        }

        /// <summary>
        /// Gets the type of the JSON5 value.
        /// </summary>
        public override Json5Type Type
        {
            get { return Json5Type.Boolean; }
        }

        /// <summary>
        /// Gets the boolean value.
        /// </summary>
        protected override object Value
        {
            get { return this.value; }
        }

        /// <summary>
        /// Converts the boolean value to a JSON5 string.
        /// </summary>
        /// <param name="space">The string to use for indentation.</param>
        /// <param name="indent">The current indentation level.</param>
        /// <param name="useOneSpaceIndent">Whether to use one space for indentation.</param>
        /// <returns>A JSON5 string representation of the boolean value.</returns>
        internal override string ToJson5String(string space, string indent, bool useOneSpaceIndent = false)
        {
            return AddIndent(this.value.ToString().ToLower(), indent, useOneSpaceIndent);
        }

        /// <summary>
        /// Defines an implicit conversion of a <see cref="Json5Boolean"/> to a <see cref="bool"/>.
        /// </summary>
        /// <param name="value">The <see cref="Json5Boolean"/> to convert.</param>
        public static implicit operator bool(Json5Boolean value)
        {
            return value.value;
        }
    }
}
