using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Json5.Tests.Stringifying
{
    /// <summary>
    /// Contains unit tests for JSON5 boolean stringification.
    /// </summary>
    [TestClass]
    public class BooleanTests
    {
        /// <summary>
        /// Tests stringification of the boolean value true.
        /// </summary>
        [TestMethod]
        public void TrueTest()
        {
            var s = Json5.Stringify(true);
            Assert.AreEqual("true", s);
        }

        /// <summary>
        /// Tests stringification of the boolean value false.
        /// </summary>
        [TestMethod]
        public void FalseTest()
        {
            var s = Json5.Stringify(false);
            Assert.AreEqual("false", s);
        }
    }
}
