using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Json5.Tests.Parsing
{
    /// <summary>
    /// Contains unit tests for JSON5 array parsing.
    /// </summary>
    [TestClass]
    public class ArrayTests
    {
        /// <summary>
        /// Tests parsing of empty arrays.
        /// </summary>
        [TestMethod]
        public void EmptyArraysTest()
        {
            var v = Json5.Parse("[]");
            var a = (Json5Array)v;
            Assert.AreEqual(0, a.Count);
        }

        /// <summary>
        /// Tests parsing of arrays with a single value.
        /// </summary>
        [TestMethod]
        public void ArrayValuesTest()
        {
            var v = Json5.Parse("[1]");
            var a = (Json5Array)v;
            Assert.AreEqual(1, a.Count);
            Assert.AreEqual(1D, (double)a[0]);
        }

        /// <summary>
        /// Tests parsing of arrays with multiple values.
        /// </summary>
        [TestMethod]
        public void MultipleArrayValuesTest()
        {
            var v = Json5.Parse("[1,2]");
            var a = (Json5Array)v;
            Assert.AreEqual(2, a.Count);
            Assert.AreEqual(1D, (double)a[0]);
            Assert.AreEqual(2D, (double)a[1]);
        }

        /// <summary>
        /// Tests parsing of nested arrays.
        /// </summary>
        [TestMethod]
        public void NestedArraysTest()
        {
            var v = Json5.Parse("[1,[2,3]]");
            var a = (Json5Array)v;
            Assert.AreEqual(2, a.Count);
            Assert.AreEqual(1D, (double)a[0]);
            Assert.AreEqual(2D, (double)a[1][0]);
            Assert.AreEqual(3D, (double)a[1][1]);
        }
    }
}
