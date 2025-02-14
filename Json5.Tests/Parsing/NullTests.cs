using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Json5.Tests.Parsing
{
    /// <summary>
    /// Contains unit tests for JSON5 null value parsing.
    /// </summary>
    [TestClass]
    public class NullTests
    {
        /// <summary>
        /// Tests parsing of the null value.
        /// </summary>
        [TestMethod]
        public void NullTest()
        {
            var v = Json5.Parse("null");
            Assert.AreEqual(Json5Type.Null, v.Type);
        }
    }
}
