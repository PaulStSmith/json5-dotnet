namespace Json5.Parsing
{
    /// <summary>  
    /// Represents the type of a JSON5 token.  
    /// </summary>  
    enum Json5TokenType
    {
        /// <summary>  
        /// End of file token.  
        /// </summary>  
        Eof,

        /// <summary>  
        /// Identifier token.  
        /// </summary>  
        Identifier,

        /// <summary>  
        /// Number token.  
        /// </summary>  
        Number,

        /// <summary>  
        /// Punctuator token.  
        /// </summary>  
        Punctuator,

        /// <summary>  
        /// String token.  
        /// </summary>  
        String,
    }
}
