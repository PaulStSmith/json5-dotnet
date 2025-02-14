using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Json5.Tests.Parsing
{
    /// <summary>
    /// Contains unit tests for JSON5 number parsing.
    /// </summary>
    [TestClass]
    public class NumberTests
    {
        /// <summary>
        /// Tests parsing of numbers with leading zeroes.
        /// </summary>
        [TestMethod]
        public void LeadingZeroesTest()
        {
            var v = Json5.Parse("[0,0.,0e0]");
            Assert.AreEqual(0D, (double)v[0]);
            Assert.AreEqual(0D, (double)v[1]);
            Assert.AreEqual(0D, (double)v[2]);
        }

        /// <summary>
        /// Tests parsing of integer numbers.
        /// </summary>
        [TestMethod]
        public void IntegersTest()
        {
            var v = Json5.Parse("[1,23,456,7890]");
            Assert.AreEqual(1D, (double)v[0]);
            Assert.AreEqual(23D, (double)v[1]);
            Assert.AreEqual(456D, (double)v[2]);
            Assert.AreEqual(7890D, (double)v[3]);
        }

        /// <summary>
        /// Tests parsing of signed numbers.
        /// </summary>
        [TestMethod]
        public void SignedNumbersTest()
        {
            var v = Json5.Parse("[-1,+2,-.1,-0]");
            Assert.AreEqual(-1D, (double)v[0]);
            Assert.AreEqual(2D, (double)v[1]);
            Assert.AreEqual(-0.1D, (double)v[2]);
            Assert.AreEqual(-0D, (double)v[3]);
        }

        /// <summary>
        /// Tests parsing of numbers with leading decimal points.
        /// </summary>
        [TestMethod]
        public void LeadingDecimalPointsTest()
        {
            var v = Json5.Parse("[.1,.23]");
            Assert.AreEqual(0.1D, (double)v[0]);
            Assert.AreEqual(0.23D, (double)v[1]);
        }

        /// <summary>
        /// Tests parsing of fractional numbers.
        /// </summary>
        [TestMethod]
        public void FractionalNumbersTest()
        {
            var v = Json5.Parse("[1.0,1.23]");
            Assert.AreEqual(1D, (double)v[0]);
            Assert.AreEqual(1.23D, (double)v[1]);
        }

        /// <summary>
        /// Tests parsing of numbers with exponents.
        /// </summary>
        [TestMethod]
        public void ExponentsTest()
        {
            var v = Json5.Parse("[1e0,1e1,1e01,1.e0,1.1e0,1e-1,1e+1]");
            Assert.AreEqual(1D, (double)v[0]);
            Assert.AreEqual(10D, (double)v[1]);
            Assert.AreEqual(10D, (double)v[2]);
            Assert.AreEqual(1D, (double)v[3]);
            Assert.AreEqual(1.1D, (double)v[4]);
            Assert.AreEqual(0.1D, (double)v[5]);
            Assert.AreEqual(10D, (double)v[6]);
        }

        /// <summary>
        /// Tests parsing of hexadecimal numbers.
        /// </summary>
        [TestMethod]
        public void HexadecimalNumbersTest()
        {
            var v = Json5.Parse("[0x1,0x10,0xff,0xFF]");
            Assert.AreEqual(1D, (double)v[0]);
            Assert.AreEqual(16D, (double)v[1]);
            Assert.AreEqual(255D, (double)v[2]);
            Assert.AreEqual(255D, (double)v[3]);
        }

        /// <summary>
        /// Tests parsing of Infinity values.
        /// </summary>
        [TestMethod]
        public void InfinityTest()
        {
            var v = Json5.Parse("[Infinity,-Infinity]");
            Assert.AreEqual(double.PositiveInfinity, (double)v[0]);
            Assert.AreEqual(double.NegativeInfinity, (double)v[1]);
        }

        /// <summary>
        /// Tests parsing of NaN values.
        /// </summary>
        [TestMethod]
        public void NaNTest()
        {
            var v = Json5.Parse("[NaN,-NaN]");
            Assert.AreEqual(double.NaN, (double)v[0]);
            Assert.AreEqual(double.NaN, (double)v[1]);
        }
    }
}
