namespace Json5
{
    /// <summary>  
    /// Represents a JSON5 null value.  
    /// </summary>  
    public class Json5Null : Json5Value
    {
        /// <summary>  
        /// Initializes a new instance of the <see cref="Json5Null"/> class.  
        /// </summary>  
        internal Json5Null() { }

        /// <summary>  
        /// Gets the type of the JSON5 value.  
        /// </summary>  
        public override Json5Type Type
        {
            get { return Json5Type.Null; }
        }

        /// <summary>  
        /// Converts the null value to a JSON5 string.  
        /// </summary>  
        /// <param name="space">The string to use for indentation.</param>  
        /// <param name="indent">The current indentation level.</param>  
        /// <param name="useOneSpaceIndent">Whether to use one space for indentation.</param>  
        /// <returns>A JSON5 string representation of the null value.</returns>  
        internal override string ToJson5String(string space, string indent, bool useOneSpaceIndent = false)
        {
            return AddIndent("null", indent, useOneSpaceIndent);
        }
    }
}
