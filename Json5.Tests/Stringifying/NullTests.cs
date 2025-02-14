using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Json5.Tests.Stringifying
{
    /// <summary>
    /// Contains unit tests for JSON5 null value stringification.
    /// </summary>
    [TestClass]
    public class NullTests
    {
        /// <summary>
        /// Tests stringification of the null value.
        /// </summary>
        [TestMethod]
        public void NullTest()
        {
            var s = Json5.Stringify(Json5Value.Null);
            Assert.AreEqual("null", s, "Expected the stringified value to be 'null' for the null value.");
        }
    }
}
