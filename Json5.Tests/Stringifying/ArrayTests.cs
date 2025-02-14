using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Json5.Tests.Stringifying
{
    /// <summary>
    /// Contains unit tests for JSON5 array stringification.
    /// </summary>
    [TestClass]
    public class ArrayTests
    {
        /// <summary>
        /// Tests stringification of empty arrays.
        /// </summary>
        [TestMethod]
        public void EmptyArraysTest()
        {
            var s = Json5.Stringify(new Json5Array());
            Assert.AreEqual("[]", s);
        }

        /// <summary>
        /// Tests stringification of arrays with a single value.
        /// </summary>
        [TestMethod]
        public void ArrayValuesTest()
        {
            var s = Json5.Stringify(new Json5Array { 1 });
            Assert.AreEqual("[1]", s);
        }

        /// <summary>
        /// Tests stringification of arrays with multiple values.
        /// </summary>
        [TestMethod]
        public void MultipleArrayValuesTest()
        {
            var s = Json5.Stringify(new Json5Array { 1, 2 });
            Assert.AreEqual("[1,2]", s);
        }

        /// <summary>
        /// Tests stringification of nested arrays.
        /// </summary>
        [TestMethod]
        public void NestedArraysTest()
        {
            var s = Json5.Stringify(new Json5Array { 1, new Json5Array { 2, 3 } });
            Assert.AreEqual("[1,[2,3]]", s);
        }

        /// <summary>
        /// Tests stringification of circular arrays.
        /// </summary>
        [TestMethod]
        public void CircularArraysTest()
        {
            var a = new Json5Array();
            a["a"] = a;
            //Json5.Stringify(a);
            Assert.Fail("Not Implemented");
        }
    }
}
