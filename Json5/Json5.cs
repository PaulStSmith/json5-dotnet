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
            var parser = new Json5Parser(new StringReader(text));
            var value = parser.Parse();

            if (reviver != null)
                return TransformParsed(value, reviver);

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
            var parser = new Json5Parser(new StringReader(text));
            var value = parser.Parse();

            if (reviver != null)
                return TransformParsed(value, (t, k, v) => reviver(k, v));

            return value;
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
                value = TransformForStringify(value, replacer);

            return value.ToJson5String(space);
        }

        /// <summary>  
        /// Converts a JSON5 value to a JSON5 string with indentation.  
        /// </summary>  
        /// <param name="value">The JSON5 value to convert.</param>  
        /// <param name="replacer">A function to transform the values during stringification.</param>  
        /// <param name="space">The number of spaces to use for indentation.</param>  
        /// <returns>A JSON5 string.</returns>  
        public static string Stringify(Json5Value value, Func<Json5Container, string, Json5Value, Json5Value> replacer, int space) => Stringify(value, replacer, GetIndent(space));

        /// <summary>  
        /// Converts a JSON5 value to a JSON5 string with a reviver function.  
        /// </summary>  
        /// <param name="value">The JSON5 value to convert.</param>  
        /// <param name="replacer">A function to transform the values during stringification.</param>  
        /// <param name="space">An optional string to use for indentation.</param>  
        /// <returns>A JSON5 string.</returns>  
        public static string Stringify(Json5Value value, Func<string, Json5Value, Json5Value> replacer, string space = null)
        {
            Func<Json5Container, string, Json5Value, Json5Value> finalReplacer = null;
            if (replacer != null)
                finalReplacer = (t, k, v) => replacer(k, v);

            return Stringify(value, finalReplacer, space);
        }

        /// <summary>  
        /// Converts a JSON5 value to a JSON5 string with a reviver function and indentation.  
        /// </summary>  
        /// <param name="value">The JSON5 value to convert.</param>  
        /// <param name="replacer">A function to transform the values during stringification.</param>  
        /// <param name="space">The number of spaces to use for indentation.</param>  
        /// <returns>A JSON5 string.</returns>  
        public static string Stringify(Json5Value value, Func<string, Json5Value, Json5Value> replacer, int space) => Stringify(value, replacer, new string(' ', Math.Min(space, 10)));

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
        public static string Stringify(Json5Value value, IEnumerable<string> keys, int space) => Stringify(value, keys, GetIndent(space));

        /// <summary>  
        /// Converts a JSON5 value to a JSON5 string.  
        /// </summary>  
        /// <param name="value">The JSON5 value to convert.</param>  
        /// <param name="space">An optional string to use for indentation.</param>  
        /// <returns>A JSON5 string.</returns>  
        public static string Stringify(Json5Value value, string space = null) => Stringify(value, (Func<Json5Container, string, Json5Value, Json5Value>)null, space);

        /// <summary>  
        /// Converts a JSON5 value to a JSON5 string with indentation.  
        /// </summary>  
        /// <param name="value">The JSON5 value to convert.</param>  
        /// <param name="space">The number of spaces to use for indentation.</param>  
        /// <returns>A JSON5 string.</returns>  
        public static string Stringify(Json5Value value, int space) => Stringify(value, GetIndent(space));

        /// <summary>  
        /// Transforms a JSON5 value using a transformer function.  
        /// </summary>  
        /// <param name="value">The JSON5 value to transform.</param>  
        /// <param name="transformer">The transformer function.</param>  
        /// <returns>The transformed JSON5 value.</returns>  
        private static Json5Value TransformParsed(Json5Value value, Func<Json5Container, string, Json5Value, Json5Value> transformer)
        {
            var holder = new Json5Object
            {
                [""] = value
            };
            return WalkWithRoot(holder, transformer); // This version allows root transformation
        }
        private static string GetIndent(int space) => new(' ', space > 10 ? 10 : space);

        /// <summary>  
        /// Transforms a JSON5 value using a transformer function.  
        /// </summary>  
        /// <param name="value">The JSON5 value to transform.</param>  
        /// <param name="transformer">The transformer function.</param>  
        /// <returns>The transformed JSON5 value.</returns>  
        private static Json5Value TransformForStringify(Json5Value value, Func<Json5Container, string, Json5Value, Json5Value> transformer)
        {
            var holder = new Json5Object
            {
                [""] = value
            };
            return WalkNoRoot(holder, transformer); // This version skips root transformation
        }

        private static Json5Value WalkWithRoot(Json5Object holder, Func<Json5Container, string, Json5Value, Json5Value> transformer)
        {
            var value = holder[""];
            TransformValue(transformer, value);
            // For root transformation, we apply the transformer after walking the tree
            return transformer(holder, "", value);
        }

        private static Json5Value WalkNoRoot(Json5Object holder, Func<Json5Container, string, Json5Value, Json5Value> transformer)
        {
            // For stringify, we don't transform the root value, just return it after walking
            var value = holder[""];
            TransformValue(transformer, value);
            return value;
        }

        // The shared helper function for walking containers remains the same
        private static Json5Value Walk(Json5Container holder, string key, Func<Json5Container, string, Json5Value, Json5Value> transformer)
        {
            var value = holder[key];
            TransformValue(transformer, value);
            return transformer(holder, key, value);
        }

        private static void TransformValue(Func<Json5Container, string, Json5Value, Json5Value> transformer, Json5Value value)
        {
            if (value is Json5Container c)
            {
                var keys = c.Keys.ToArray();
                foreach (var k in keys)
                {
                    var v = Walk(c, k, transformer);
                    if (v != null || value is Json5Array)
                        c[k] = v;
                    else
                        c.Remove(k);
                }
            }
        }

        /// <summary>  
        /// Quotes a string for JSON5 output.  
        /// </summary>  
        /// <param name="s">The string to quote.</param>  
        /// <returns>The quoted string.</returns>  
        internal static string QuoteString(string s)
        {
            var doubleQuotes = 0;
            var singleQuotes = 0;
            foreach (var c in s)
            {
                if (c == '"')
                    doubleQuotes++;
                else if (c == '\'')
                    singleQuotes++;
            }

            var quote = doubleQuotes >= singleQuotes ? '\'' : '"';
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
            var r = "";
            foreach (var c in s)
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
                case '\0': return "\\0";
                case '\b': return "\\b";
                case '\t': return "\\t";
                case '\n': return "\\n";
                case '\f': return "\\f";
                case '\r': return "\\r";
                case '\v': return "\\v";
                case '\\': return "\\\\";
                case '\u2028': return "\\u2028";
                case '\u2029': return "\\u2029";
            }

            if (c < ' ')
            {
                return "\\x" + ((int)c).ToString("x2");
            }

            return char.GetUnicodeCategory(c) switch
            {
                UnicodeCategory.Control or
                UnicodeCategory.Format or
                UnicodeCategory.Surrogate or
                UnicodeCategory.PrivateUse or
                UnicodeCategory.OtherNotAssigned => "\\u" + ((int)c).ToString("x4"),
                _ => c.ToString(),
            };
        }
    }
}
