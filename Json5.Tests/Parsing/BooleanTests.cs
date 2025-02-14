using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Json5.Tests.Parsing
{
    /// <summary>
    /// Contains unit tests for JSON5 boolean parsing.
    /// </summary>
    [TestClass]
    public class BooleanTests
    {
        /// <summary>
        /// Tests parsing of the boolean value true.
        /// </summary>
        [TestMethod]
        public void TrueTest()
        {
            var v = Json5.Parse("true");
            Assert.IsTrue((bool)v, "Expected the parsed value to be true.");
        }

        /// <summary>
        /// Tests parsing of the boolean value false.
        /// </summary>
        [TestMethod]
        public void FalseTest()
        {
            var v = Json5.Parse("false");
            Assert.IsFalse((bool)v, "Expected the parsed value to be false.");
        }
    }
}
