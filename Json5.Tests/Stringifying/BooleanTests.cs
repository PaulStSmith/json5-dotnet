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
            Assert.AreEqual("true", s, "Expected the stringified value to be 'true' for the boolean value true.");
        }

        /// <summary>
        /// Tests stringification of the boolean value false.
        /// </summary>
        [TestMethod]
        public void FalseTest()
        {
            var s = Json5.Stringify(false);
            Assert.AreEqual("false", s, "Expected the stringified value to be 'false' for the boolean value false.");
        }
    }
}
