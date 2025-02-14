using System;

namespace Json5
{
    /// <summary>  
    /// Represents a JSON5 value.  
    /// </summary>  
    public abstract class Json5Value
    {
        /// <summary>  
        /// Represents a JSON5 null value.  
        /// </summary>  
        public static readonly Json5Null Null = new Json5Null();

        /// <summary>  
        /// Gets the type of the JSON5 value.  
        /// </summary>  
        public abstract Json5Type Type { get; }

        /// <summary>  
        /// Gets or sets the value associated with the specified key.  
        /// </summary>  
        /// <param name="key">The key of the value to get or set.</param>  
        /// <returns>The value associated with the specified key.</returns>  
        /// <exception cref="NotSupportedException">Thrown when the operation is not supported.</exception>  
        public virtual Json5Value this[string key]
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        /// <summary>  
        /// Gets or sets the value at the specified index.  
        /// </summary>  
        /// <param name="index">The zero-based index of the value to get or set.</param>  
        /// <returns>The value at the specified index.</returns>  
        /// <exception cref="NotSupportedException">Thrown when the operation is not supported.</exception>  
        public virtual Json5Value this[int index]
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        /// <summary>  
        /// Returns a string that represents the current object.  
        /// </summary>  
        /// <returns>A string that represents the current object.</returns>  
        public override string ToString()
        {
            return this.ToJson5String();
        }

        /// <summary>  
        /// Converts the value to a JSON5 string.  
        /// </summary>  
        /// <param name="space">The string to use for indentation.</param>  
        /// <returns>A JSON5 string representation of the value.</returns>  
        public string ToJson5String(string space = null)
        {
            if (space != null && space.Length > 10)
                space = space.Remove(10);

            return this.ToJson5String(space, "");
        }

        /// <summary>  
        /// Converts the value to a JSON5 string.  
        /// </summary>  
        /// <param name="space">The string to use for indentation.</param>  
        /// <param name="indent">The current indentation level.</param>  
        /// <param name="useOneSpaceIndent">Whether to use one space for indentation.</param>  
        /// <returns>A JSON5 string representation of the value.</returns>  
        internal abstract string ToJson5String(string space, string indent, bool useOneSpaceIndent = false);

        /// <summary>  
        /// Adds an indent string to the start of the original string, if needed.  
        /// </summary>  
        /// <param name="originalString">The original string.</param>  
        /// <param name="indent">The indent string, which can contain characters other than whitespace.</param>  
        /// <param name="useOneSpaceIndent">Whether to use one space for indentation.</param>  
        /// <returns>The original or combined string.</returns>  
        internal static string AddIndent(string originalString, string indent, bool useOneSpaceIndent)
        {
            if (useOneSpaceIndent)
            {
                return " " + originalString;
            }
            else if (string.IsNullOrEmpty(indent))
            {
                return originalString;
            }

            return indent + originalString;
        }

        /// <summary>  
        /// Defines an implicit conversion of a <see cref="string"/> to a <see cref="Json5Value"/>.  
        /// </summary>  
        /// <param name="value">The string value to convert.</param>  
        public static implicit operator Json5Value(string value)
        {
            if (value == null)
                return null;

            return new Json5String(value);
        }

        /// <summary>  
        /// Defines an implicit conversion of a <see cref="double"/> to a <see cref="Json5Value"/>.  
        /// </summary>  
        /// <param name="value">The double value to convert.</param>  
        public static implicit operator Json5Value(double value)
        {
            return new Json5Number(value);
        }

        /// <summary>  
        /// Defines an implicit conversion of a nullable <see cref="double"/> to a <see cref="Json5Value"/>.  
        /// </summary>  
        /// <param name="value">The nullable double value to convert.</param>  
        public static implicit operator Json5Value(double? value)
        {
            if (value.HasValue)
                return new Json5Number(value.Value);

            return null;
        }

        /// <summary>  
        /// Defines an implicit conversion of a <see cref="bool"/> to a <see cref="Json5Value"/>.  
        /// </summary>  
        /// <param name="value">The boolean value to convert.</param>  
        public static implicit operator Json5Value(bool value)
        {
            return new Json5Boolean(value);
        }

        /// <summary>  
        /// Defines an implicit conversion of a nullable <see cref="bool"/> to a <see cref="Json5Value"/>.  
        /// </summary>  
        /// <param name="value">The nullable boolean value to convert.</param>  
        public static implicit operator Json5Value(bool? value)
        {
            if (value.HasValue)
                return new Json5Boolean(value.Value);

            return null;
        }

        /// <summary>  
        /// Defines an implicit conversion of a <see cref="DateTimeOffset"/> to a <see cref="Json5Value"/>.  
        /// </summary>  
        /// <param name="value">The DateTimeOffset value to convert.</param>  
        public static implicit operator Json5Value(DateTimeOffset value)
        {
            return new Json5Date(value);
        }

        /// <summary>  
        /// Defines an implicit conversion of a nullable <see cref="DateTimeOffset"/> to a <see cref="Json5Value"/>.  
        /// </summary>  
        /// <param name="value">The nullable DateTimeOffset value to convert.</param>  
        public static implicit operator Json5Value(DateTimeOffset? value)
        {
            if (value.HasValue)
                return new Json5Date(value.Value);

            return null;
        }

        /// <summary>  
        /// Defines an implicit conversion of a <see cref="DateTime"/> to a <see cref="Json5Value"/>.  
        /// </summary>  
        /// <param name="value">The DateTime value to convert.</param>  
        public static implicit operator Json5Value(DateTime value)
        {
            return new Json5Date(value);
        }

        /// <summary>  
        /// Defines an implicit conversion of a nullable <see cref="DateTime"/> to a <see cref="Json5Value"/>.  
        /// </summary>  
        /// <param name="value">The nullable DateTime value to convert.</param>  
        public static implicit operator Json5Value(DateTime? value)
        {
            if (value.HasValue)
                return new Json5Date(value.Value);

            return null;
        }

        /// <summary>  
        /// Defines an explicit conversion of a <see cref="Json5Value"/> to a <see cref="string"/>.  
        /// </summary>  
        /// <param name="value">The Json5Value to convert.</param>  
        public static explicit operator string(Json5Value value)
        {
            return (Json5String)value;
        }

        /// <summary>  
        /// Defines an explicit conversion of a <see cref="Json5Value"/> to a <see cref="double"/>.  
        /// </summary>  
        /// <param name="value">The Json5Value to convert.</param>  
        public static explicit operator double(Json5Value value)
        {
            return (Json5Number)value;
        }

        /// <summary>  
        /// Defines an explicit conversion of a <see cref="Json5Value"/> to a nullable <see cref="double"/>.  
        /// </summary>  
        /// <param name="value">The Json5Value to convert.</param>  
        public static explicit operator double?(Json5Value value)
        {
            return (Json5Number)value;
        }

        /// <summary>  
        /// Defines an explicit conversion of a <see cref="Json5Value"/> to a <see cref="bool"/>.  
        /// </summary>  
        /// <param name="value">The Json5Value to convert.</param>  
        public static explicit operator bool(Json5Value value)
        {
            return (Json5Boolean)value;
        }

        /// <summary>  
        /// Defines an explicit conversion of a <see cref="Json5Value"/> to a nullable <see cref="bool"/>.  
        /// </summary>  
        /// <param name="value">The Json5Value to convert.</param>  
        public static explicit operator bool?(Json5Value value)
        {
            return (Json5Boolean)value;
        }

        /// <summary>  
        /// Defines an explicit conversion of a <see cref="Json5Value"/> to a <see cref="DateTimeOffset"/>.  
        /// </summary>  
        /// <param name="value">The Json5Value to convert.</param>  
        public static explicit operator DateTimeOffset(Json5Value value)
        {
            return (Json5Date)value;
        }

        /// <summary>  
        /// Defines an explicit conversion of a <see cref="Json5Value"/> to a nullable <see cref="DateTimeOffset"/>.  
        /// </summary>  
        /// <param name="value">The Json5Value to convert.</param>  
        public static explicit operator DateTimeOffset?(Json5Value value)
        {
            return (Json5Date)value;
        }

        /// <summary>  
        /// Defines an explicit conversion of a <see cref="Json5Value"/> to a <see cref="DateTime"/>.  
        /// </summary>  
        /// <param name="value">The Json5Value to convert.</param>  
        public static explicit operator DateTime(Json5Value value)
        {
            return (Json5Date)value;
        }

        /// <summary>  
        /// Defines an explicit conversion of a <see cref="Json5Value"/> to a nullable <see cref="DateTime"/>.  
        /// </summary>  
        /// <param name="value">The Json5Value to convert.</param>  
        public static explicit operator DateTime?(Json5Value value)
        {
            return (Json5Date)value;
        }
    }
}
