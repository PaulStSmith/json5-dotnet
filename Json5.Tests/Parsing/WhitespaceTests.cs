using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Json5.Tests.Parsing
{
    /// <summary>
    /// Contains unit tests for JSON5 whitespace parsing.
    /// </summary>
    [TestClass]
    public class WhitespaceTests
    {
        /// <summary>
        /// Tests parsing of various whitespace characters.
        /// </summary>
        [TestMethod]
        public void WhitespaceTest()
        {
            var v = Json5.Parse("{\t\v\f \u00A0\uFEFF\n\r\u2028\u2029\u2003}");
            var o = (Json5Object)v;
            Assert.AreEqual(0, o.Count, "Expected the object to have no properties after parsing various whitespace characters.");
        }
    }
}
