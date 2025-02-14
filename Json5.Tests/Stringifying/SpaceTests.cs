using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Json5.Tests.Stringifying
{
    /// <summary>
    /// Contains unit tests for JSON5 stringification with space formatting.
    /// </summary>
    [TestClass]
    public class SpaceTests
    {
        /// <summary>
        /// Tests stringification with null space.
        /// </summary>
        [TestMethod]
        public void NullSpaceTest()
        {
            var s = Json5.Stringify(new Json5Array { 1 });
            Assert.AreEqual("[1]", s);
        }

        /// <summary>
        /// Tests stringification with zero space.
        /// </summary>
        [TestMethod]
        public void ZeroSpaceTest()
        {
            var s = Json5.Stringify(new Json5Array { 1 }, 0);
            Assert.AreEqual("[1]", s);
        }

        /// <summary>
        /// Tests stringification with empty string space.
        /// </summary>
        [TestMethod]
        public void EmptyStringSpaceTest()
        {
            var s = Json5.Stringify(new Json5Array { 1 }, "");
            Assert.AreEqual("[1]", s);
        }

        /// <summary>
        /// Tests stringification with numeric space.
        /// </summary>
        [TestMethod]
        public void NumberSpaceTest()
        {
            var s = Json5.Stringify(new Json5Array { 1 }, 2);
            Assert.AreEqual("[\n  1,\n]", s);
        }

        /// <summary>
        /// Tests stringification with maximum numeric space.
        /// </summary>
        [TestMethod]
        public void MaxNumberSpaceTest()
        {
            var s = Json5.Stringify(new Json5Array { 1 }, 11);
            Assert.AreEqual("[\n          1,\n]", s);
        }

        /// <summary>
        /// Tests stringification with string space.
        /// </summary>
        [TestMethod]
        public void StringSpaceTest()
        {
            var s = Json5.Stringify(new Json5Array { 1 }, "\t");
            Assert.AreEqual("[\n\t1,\n]", s);
        }

        /// <summary>
        /// Tests stringification with maximum string space.
        /// </summary>
        [TestMethod]
        public void MaxStringSpaceTest()
        {
            var s = Json5.Stringify(new Json5Array { 1 }, "           ");
            Assert.AreEqual("[\n          1,\n]", s);
        }

        /// <summary>
        /// Tests stringification of arrays with space formatting.
        /// </summary>
        [TestMethod]
        public void ArraysTest()
        {
            var s = Json5.Stringify(new Json5Array { 1 }, 2);
            Assert.AreEqual("[\n  1,\n]", s);
        }

        /// <summary>
        /// Tests stringification of nested arrays with space formatting.
        /// </summary>
        [TestMethod]
        public void NestedArraysTest()
        {
            var s = Json5.Stringify(new Json5Array { 1, new Json5Array { 2 }, 3 }, 2);
            Assert.AreEqual("[\n  1,\n  [\n    2,\n  ],\n  3,\n]", s);
        }

        /// <summary>
        /// Tests stringification of objects with space formatting.
        /// </summary>
        [TestMethod]
        public void ObjectsTest()
        {
            var s = Json5.Stringify(new Json5Object { { "a", 1 } }, 2);
            Assert.AreEqual("{\n  a: 1,\n}", s);
        }

        /// <summary>
        /// Tests stringification of nested objects with space formatting.
        /// </summary>
        [TestMethod]
        public void NestedObjectsTest()
        {
            var s = Json5.Stringify(new Json5Object { { "a", new Json5Object { { "b", 2 } } } }, 2);
            Assert.AreEqual("{\n  a: {\n    b: 2,\n  },\n}", s);
        }
    }
}
