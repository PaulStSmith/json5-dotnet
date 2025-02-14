using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Json5.Tests.Stringifying
{
    /// <summary>
    /// Contains unit tests for JSON5 number stringification.
    /// </summary>
    [TestClass]
    public class NumberTests
    {
        /// <summary>
        /// Tests stringification of numeric values.
        /// </summary>
        [TestMethod]
        public void NumbersTest()
        {
            var s = Json5.Stringify(-1.2);
            Assert.AreEqual("-1.2", s);
        }

        /// <summary>
        /// Tests stringification of non-finite numeric values.
        /// </summary>
        [TestMethod]
        public void NonFiniteNumbersTest()
        {
            var s = Json5.Stringify(new Json5Array { double.PositiveInfinity, double.NegativeInfinity, double.NaN });
            Assert.AreEqual("[Infinity,-Infinity,NaN]", s);
        }
    }
}
