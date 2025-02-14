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
            Assert.AreEqual(0, a.Count, "The array should be empty.");
        }

        /// <summary>
        /// Tests parsing of arrays with a single value.
        /// </summary>
        [TestMethod]
        public void ArrayValuesTest()
        {
            var v = Json5.Parse("[1]");
            var a = (Json5Array)v;
            Assert.AreEqual(1, a.Count, "The array should contain one element.");
            Assert.AreEqual(1D, (double)a[0], "The first element should be 1.");
        }

        /// <summary>
        /// Tests parsing of arrays with multiple values.
        /// </summary>
        [TestMethod]
        public void MultipleArrayValuesTest()
        {
            var v = Json5.Parse("[1,2]");
            var a = (Json5Array)v;
            Assert.AreEqual(2, a.Count, "The array should contain two elements.");
            Assert.AreEqual(1D, (double)a[0], "The first element should be 1.");
            Assert.AreEqual(2D, (double)a[1], "The second element should be 2.");
        }

        /// <summary>
        /// Tests parsing of nested arrays.
        /// </summary>
        [TestMethod]
        public void NestedArraysTest()
        {
            var v = Json5.Parse("[1,[2,3]]");
            var a = (Json5Array)v;
            Assert.AreEqual(2, a.Count, "The array should contain two elements.");
            Assert.AreEqual(1D, (double)a[0], "The first element should be 1.");
            Assert.AreEqual(2D, (double)a[1][0], "The first element of the nested array should be 2.");
            Assert.AreEqual(3D, (double)a[1][1], "The second element of the nested array should be 3.");
        }
    }
}
