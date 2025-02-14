using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Json5.Tests.Stringifying
{
    /// <summary>
    /// Contains unit tests for JSON5 string stringification.
    /// </summary>
    [TestClass]
    public class StringTests
    {
        /// <summary>
        /// Tests stringification of single-quoted strings.
        /// </summary>
        [TestMethod]
        public void SingleQuotedStringsTest()
        {
            var s = Json5.Stringify("abc");
            Assert.AreEqual("'abc'", s);
        }

        /// <summary>
        /// Tests stringification of double-quoted strings.
        /// </summary>
        [TestMethod]
        public void DoubleQuotedStringsTest()
        {
            var s = Json5.Stringify("abc'");
            Assert.AreEqual("\"abc'\"", s);
        }

        /// <summary>
        /// Tests stringification of strings with escaped characters.
        /// </summary>
        [TestMethod]
        public void EscapedCharactersTest()
        {
            var s = Json5.Stringify("\\\b\f\n\r\t\v\0\x0f");
            Assert.AreEqual(@"'\\\b\f\n\r\t\v\0\x0f'", s);
        }

        /// <summary>
        /// Tests stringification of strings with escaped single quotes.
        /// </summary>
        [TestMethod]
        public void EscapedSingleQuotesTest()
        {
            var s = Json5.Stringify("'\"");
            Assert.AreEqual("'\\'\"'", s);
        }

        /// <summary>
        /// Tests stringification of strings with escaped double quotes.
        /// </summary>
        [TestMethod]
        public void EscapedDoubleQuotesTest()
        {
            var s = Json5.Stringify("''\"");
            Assert.AreEqual("\"''\\\"\"", s);
        }

        /// <summary>
        /// Tests stringification of strings with escaped separator characters.
        /// </summary>
        [TestMethod]
        public void EscapedSeparatorsTest()
        {
            var s = Json5.Stringify("\u2028\u2029");
            Assert.AreEqual("'\\u2028\\u2029'", s);
        }
    }
}
