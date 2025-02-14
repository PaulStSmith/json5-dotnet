using System;

namespace Json5
{
    /// <summary>
    /// Represents a JSON5 date value.
    /// </summary>
    public class Json5Date : Json5Primitive
    {
        private readonly DateTimeOffset value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Json5Date"/> class with the specified value.
        /// </summary>
        /// <param name="value">The date value.</param>
        public Json5Date(DateTimeOffset value)
        {
            this.value = value;
        }

        /// <summary>
        /// Gets the type of the JSON5 value.
        /// </summary>
        public override Json5Type Type
        {
            get { return Json5Type.Date; }
        }

        /// <summary>
        /// Gets the date value.
        /// </summary>
        protected override object Value
        {
            get { return this.value; }
        }

        /// <summary>
        /// Converts the date value to a JSON5 string.
        /// </summary>
        /// <param name="space">The string to use for indentation.</param>
        /// <param name="indent">The current indentation level.</param>
        /// <param name="useOneSpaceIndent">Whether to use one space for indentation.</param>
        /// <returns>A JSON5 string representation of the date value.</returns>
        internal override string ToJson5String(string space, string indent, bool useOneSpaceIndent = false)
        {
            return AddIndent(Json5.QuoteString(this.value.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'")), indent, useOneSpaceIndent);
        }

        /// <summary>
        /// Defines an implicit conversion of a <see cref="Json5Date"/> to a <see cref="DateTimeOffset"/>.
        /// </summary>
        /// <param name="value">The <see cref="Json5Date"/> to convert.</param>
        public static implicit operator DateTimeOffset(Json5Date value)
        {
            return value.value;
        }

        /// <summary>
        /// Defines an implicit conversion of a <see cref="Json5Date"/> to a <see cref="DateTime"/>.
        /// </summary>
        /// <param name="value">The <see cref="Json5Date"/> to convert.</param>
        public static implicit operator DateTime(Json5Date value)
        {
            return value.value.DateTime;
        }
    }
}
