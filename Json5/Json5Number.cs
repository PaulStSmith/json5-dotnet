using System.Globalization;

namespace Json5
{
    /// <summary>  
    /// Represents a JSON5 number value.  
    /// </summary>  
    public class Json5Number : Json5Primitive
    {
        private readonly double value;

        /// <summary>  
        /// Initializes a new instance of the <see cref="Json5Number"/> class with the specified value.  
        /// </summary>  
        /// <param name="value">The numeric value.</param>  
        public Json5Number(double value)
        {
            this.value = value;
        }

        /// <summary>  
        /// Gets the type of the JSON5 value.  
        /// </summary>  
        public override Json5Type Type
        {
            get { return Json5Type.Number; }
        }

        /// <summary>  
        /// Gets the numeric value.  
        /// </summary>  
        protected override object Value
        {
            get { return this.value; }
        }

        /// <summary>  
        /// Converts the numeric value to a JSON5 string.  
        /// </summary>  
        /// <param name="space">The string to use for indentation.</param>  
        /// <param name="indent">The current indentation level.</param>  
        /// <param name="useOneSpaceIndent">Whether to use one space for indentation.</param>  
        /// <returns>A JSON5 string representation of the numeric value.</returns>  
        internal override string ToJson5String(string space, string indent, bool useOneSpaceIndent = false)
        {
            return AddIndent(this.value.ToString(CultureInfo.InvariantCulture), indent, useOneSpaceIndent);
        }

        /// <summary>  
        /// Defines an implicit conversion of a <see cref="Json5Number"/> to a <see cref="double"/>.  
        /// </summary>  
        /// <param name="value">The <see cref="Json5Number"/> to convert.</param>  
        public static implicit operator double(Json5Number value)
        {
            return value.value;
        }
    }
}
