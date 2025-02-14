using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Json5.Tests.Parsing
{
    /// <summary>
    /// Contains unit tests for JSON5 string parsing.
    /// </summary>
    [TestClass]
    public class StringTests
    {
        /// <summary>
        /// Tests parsing of double-quoted strings.
        /// </summary>
        [TestMethod]
        public void DoubleQuotedStringsTest()
        {
            var v = Json5.Parse("\"abc\"");
            Assert.AreEqual("abc", (string)v);
        }

        /// <summary>
        /// Tests parsing of single-quoted strings.
        /// </summary>
        [TestMethod]
        public void SingleQuotedStringsTest()
        {
            var v = Json5.Parse("'abc'");
            Assert.AreEqual("abc", (string)v);
        }

        /// <summary>
        /// Tests parsing of strings with nested quotes.
        /// </summary>
        [TestMethod]
        public void NestedQuotesInStringsTest()
        {
            var v = Json5.Parse("['\"',\"'\"]");
            Assert.AreEqual("\"", (string)v[0]);
            Assert.AreEqual("'", (string)v[1]);
        }

        /// <summary>
        /// Tests parsing of strings with escaped characters.
        /// </summary>
        [TestMethod]
        public void EscapedCharactersTest()
        {
            var v = Json5.Parse("'\\b\\f\\n\\r\\t\\v\\0\\x0f\\u01fF\\\n\\\r\n\\\r\\\u2028\\\u2029\\a\\'\\\"'");
            Assert.AreEqual("\b\f\n\r\t\v\0\x0f\u01FFa'\"", (string)v);
        }

        /// <summary>
        /// Tests parsing of strings with separator characters.
        /// </summary>
        [TestMethod]
        public void SeparatorsTest()
        {
            var v = Json5.Parse("'\u2028\u2029'");
            Assert.AreEqual("\u2028\u2029", (string)v);
        }
    }
}
