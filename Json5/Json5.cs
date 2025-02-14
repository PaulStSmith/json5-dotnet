using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Json5
{
    using Parsing;

    /// <summary>  
    /// Contains methods for parsing and generating JSON5 text.  
    /// </summary>  
    public static class Json5
    {
        /// <summary>  
        /// Parses JSON5 text into a JSON5 value.  
        /// </summary>  
        /// <param name="text">The text to parse.</param>  
        /// <param name="reviver">An optional function to transform the parsed values.</param>  
        /// <returns>A JSON5 value.</returns>  
        public static Json5Value Parse(string text, Func<Json5Container, string, Json5Value, Json5Value> reviver = null)
        {
            Json5Parser parser = new Json5Parser(new StringReader(text));
            Json5Value value = parser.Parse();

            if (reviver != null)
                return Transform(value, reviver);

            return value;
        }

        /// <summary>  
        /// Parses JSON5 text into a JSON5 value with a reviver function.  
        /// </summary>  
        /// <param name="text">The text to parse.</param>  
        /// <param name="reviver">A function to transform the parsed values.</param>  
        /// <returns>A JSON5 value.</returns>  
        public static Json5Value Parse(string text, Func<string, Json5Value, Json5Value> reviver)
        {
            throw new NotImplementedException();
        }

        /// <summary>  
        /// Converts a JSON5 value to a JSON5 string.  
        /// </summary>  
        /// <param name="value">The JSON5 value to convert.</param>  
        /// <param name="replacer">A function to transform the values during stringification.</param>  
        /// <param name="space">An optional string to use for indentation.</param>  
        /// <returns>A JSON5 string.</returns>  
        public static string Stringify(Json5Value value, Func<Json5Container, string, Json5Value, Json5Value> replacer, string space = null)
        {
            if (replacer != null)
                value = Transform(value, replacer);

            return value.ToJson5String(space);
        }

        /// <summary>  
        /// Converts a JSON5 value to a JSON5 string with indentation.  
        /// </summary>  
        /// <param name="value">The JSON5 value to convert.</param>  
        /// <param name="replacer">A function to transform the values during stringification.</param>  
        /// <param name="space">The number of spaces to use for indentation.</param>  
        /// <returns>A JSON5 string.</returns>  
        public static string Stringify(Json5Value value, Func<Json5Container, string, Json5Value, Json5Value> replacer, int space)
        {
            return Stringify(value, replacer, new string(' ', Math.Min(space, 10)));
        }

        /// <summary>  
        /// Converts a JSON5 value to a JSON5 string with a reviver function.  
        /// </summary>  
        /// <param name="value">The JSON5 value to convert.</param>  
        /// <param name="replacer">A function to transform the values during stringification.</param>  
        /// <param name="space">An optional string to use for indentation.</param>  
        /// <returns>A JSON5 string.</returns>  
        public static string Stringify(Json5Value value, Func<string, Json5Value, Json5Value> replacer, string space = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>  
        /// Converts a JSON5 value to a JSON5 string with a reviver function and indentation.  
        /// </summary>  
        /// <param name="value">The JSON5 value to convert.</param>  
        /// <param name="replacer">A function to transform the values during stringification.</param>  
        /// <param name="space">The number of spaces to use for indentation.</param>  
        /// <returns>A JSON5 string.</returns>  
        public static string Stringify(Json5Value value, Func<string, Json5Value, Json5Value> replacer, int space)
        {
            throw new NotImplementedException();
        }

        /// <summary>  
        /// Converts a JSON5 value to a JSON5 string with a list of keys to include.  
        /// </summary>  
        /// <param name="value">The JSON5 value to convert.</param>  
        /// <param name="keys">A list of keys to include in the output.</param>  
        /// <param name="space">An optional string to use for indentation.</param>  
        /// <returns>A JSON5 string.</returns>  
        public static string Stringify(Json5Value value, IEnumerable<string> keys, string space = null)
        {
            Func<Json5Container, string, Json5Value, Json5Value> replacer = null;
            if (keys != null)
                replacer = (t, k, v) => keys.Contains(k) ? v : null;

            return Stringify(value, replacer, space);
        }

        /// <summary>  
        /// Converts a JSON5 value to a JSON5 string with a list of keys to include and indentation.  
        /// </summary>  
        /// <param name="value">The JSON5 value to convert.</param>  
        /// <param name="keys">A list of keys to include in the output.</param>  
        /// <param name="space">The number of spaces to use for indentation.</param>  
        /// <returns>A JSON5 string.</returns>  
        public static string Stringify(Json5Value value, IEnumerable<string> keys, int space)
        {
            return Stringify(value, keys, new string(' ', Math.Max(space, 10)));
        }

        /// <summary>  
        /// Converts a JSON5 value to a JSON5 string.  
        /// </summary>  
        /// <param name="value">The JSON5 value to convert.</param>  
        /// <param name="space">An optional string to use for indentation.</param>  
        /// <returns>A JSON5 string.</returns>  
        public static string Stringify(Json5Value value, string space = null)
        {
            return Stringify(value, (Func<Json5Container, string, Json5Value, Json5Value>)null, space);
        }

        /// <summary>  
        /// Converts a JSON5 value to a JSON5 string with indentation.  
        /// </summary>  
        /// <param name="value">The JSON5 value to convert.</param>  
        /// <param name="space">The number of spaces to use for indentation.</param>  
        /// <returns>A JSON5 string.</returns>  
        public static string Stringify(Json5Value value, int space)
        {
            return Stringify(value, (Func<Json5Container, string, Json5Value, Json5Value>)null, space);
        }

        /// <summary>  
        /// Transforms a JSON5 value using a transformer function.  
        /// </summary>  
        /// <param name="value">The JSON5 value to transform.</param>  
        /// <param name="transformer">The transformer function.</param>  
        /// <returns>The transformed JSON5 value.</returns>  
        private static Json5Value Transform(Json5Value value, Func<Json5Container, string, Json5Value, Json5Value> transformer)
        {
            Json5Object holder = new Json5Object();
            holder[""] = value;
            return Walk(holder, "", transformer);
        }

        /// <summary>  
        /// Walks through a JSON5 container and applies a transformer function to its values.  
        /// </summary>  
        /// <param name="holder">The JSON5 container to walk through.</param>  
        /// <param name="key">The key of the current value.</param>  
        /// <param name="transformer">The transformer function.</param>  
        /// <returns>The transformed JSON5 value.</returns>  
        private static Json5Value Walk(Json5Container holder, string key, Func<Json5Container, string, Json5Value, Json5Value> transformer)
        {
            Json5Value value = holder[key];
            if (value is Json5Container)
            {
                Json5Container c = (Json5Container)value;
                string[] keys = c.Keys.ToArray();
                foreach (string k in keys)
                {
                    Json5Value v = Walk(c, k, transformer);
                    if (v != null)
                        c[k] = v;
                    else
                        c.Remove(k);
                }
            }

            // Special case for holder  
            if (key == "")
            {
                return value;
            }

            return transformer(holder, key, value);
        }

        /// <summary>  
        /// Quotes a string for JSON5 output.  
        /// </summary>  
        /// <param name="s">The string to quote.</param>  
        /// <returns>The quoted string.</returns>  
        internal static string QuoteString(string s)
        {
            int doubleQuotes = 0;
            int singleQuotes = 0;
            foreach (char c in s)
            {
                if (c == '"')
                    doubleQuotes++;
                else if (c == '\'')
                    singleQuotes++;
            }

            char quote = doubleQuotes >= singleQuotes ? '\'' : '"';
            return quote + EscapeString(s, quote) + quote;
        }

        /// <summary>  
        /// Escapes a string for JSON5 output.  
        /// </summary>  
        /// <param name="s">The string to escape.</param>  
        /// <param name="quote">The quote character to use.</param>  
        /// <returns>The escaped string.</returns>  
        internal static string EscapeString(string s, char quote)
        {
            string r = "";
            foreach (char c in s)
                r += EscapeChar(c, quote);

            return r;
        }

        /// <summary>  
        /// Escapes a character for JSON5 output.  
        /// </summary>  
        /// <param name="c">The character to escape.</param>  
        /// <param name="quote">The quote character to use.</param>  
        /// <returns>The escaped character.</returns>  
        private static string EscapeChar(char c, char quote)
        {
            if (c == quote)
                return "\\" + quote;

            switch (c)
            {
                case '\b': return "\\b";
                case '\t': return "\\t";
                case '\n': return "\\n";
                case '\f': return "\\f";
                case '\r': return "\\r";
                case '\\': return "\\\\";
                case '\u2028': return "\\u2028";
                case '\u2029': return "\\u2029";
            }

            switch (char.GetUnicodeCategory(c))
            {
                case UnicodeCategory.Control:
                case UnicodeCategory.Format:
                case UnicodeCategory.Surrogate:
                case UnicodeCategory.PrivateUse:
                case UnicodeCategory.OtherNotAssigned:
                    return "\\u" + ((int)c).ToString("x4");
            }

            return c.ToString();
        }
    }
}
