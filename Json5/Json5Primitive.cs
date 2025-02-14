namespace Json5
{
    /// <summary>  
    /// Represents a JSON5 primitive value.  
    /// </summary>  
    public abstract class Json5Primitive : Json5Value
    {
        /// <summary>  
        /// Gets the value of the primitive.  
        /// </summary>  
        protected abstract object Value { get; }

        /// <summary>  
        /// Returns the hash code for this instance.  
        /// </summary>  
        /// <returns>A hash code for the current object.</returns>  
        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        /// <summary>  
        /// Determines whether the specified object is equal to the current object.  
        /// </summary>  
        /// <param name="obj">The object to compare with the current object.</param>  
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>  
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            return obj is Json5Primitive o && o.Value == this.Value;
        }

        /// <summary>  
        /// Determines whether two specified instances of <see cref="Json5Primitive"/> are equal.  
        /// </summary>  
        /// <param name="a">The first instance to compare.</param>  
        /// <param name="b">The second instance to compare.</param>  
        /// <returns>true if the instances are equal; otherwise, false.</returns>  
        public static bool operator ==(Json5Primitive a, Json5Primitive b)
        {
            return a.Value == b.Value;
        }

        /// <summary>  
        /// Determines whether two specified instances of <see cref="Json5Primitive"/> are not equal.  
        /// </summary>  
        /// <param name="a">The first instance to compare.</param>  
        /// <param name="b">The second instance to compare.</param>  
        /// <returns>true if the instances are not equal; otherwise, false.</returns>  
        public static bool operator !=(Json5Primitive a, Json5Primitive b)
        {
            return !(a == b);
        }
    }
}
