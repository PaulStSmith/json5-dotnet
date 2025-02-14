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
            Assert.AreEqual("[]", s, "Expected the stringified value to be '[]' for an empty array.");
        }

        /// <summary>
        /// Tests stringification of arrays with a single value.
        /// </summary>
        [TestMethod]
        public void ArrayValuesTest()
        {
            var s = Json5.Stringify(new Json5Array { 1 });
            Assert.AreEqual("[1]", s, "Expected the stringified value to be '[1]' for an array with a single value.");
        }

        /// <summary>
        /// Tests stringification of arrays with multiple values.
        /// </summary>
        [TestMethod]
        public void MultipleArrayValuesTest()
        {
            var s = Json5.Stringify(new Json5Array { 1, 2 });
            Assert.AreEqual("[1,2]", s, "Expected the stringified value to be '[1,2]' for an array with multiple values.");
        }

        /// <summary>
        /// Tests stringification of nested arrays.
        /// </summary>
        [TestMethod]
        public void NestedArraysTest()
        {
            var s = Json5.Stringify(new Json5Array { 1, new Json5Array { 2, 3 } });
            Assert.AreEqual("[1,[2,3]]", s, "Expected the stringified value to be '[1,[2,3]]' for nested arrays.");
        }

        /// <summary>
        /// Tests stringification of circular arrays.
        /// </summary>
        [TestMethod]
        public void CircularArraysTest()
        {
            // Create an array that contains itself
            var array = new Json5Array();
            array.Add(array);

            // This should throw Json5CircularReferenceException
            Assert.ThrowsExactly<Json5CircularReferenceException>(() => Json5.Stringify(array));
        }

        /// <summary>
        /// Tests stringification of deep circular arrays.
        /// </summary>
        [TestMethod]
        public void DeepCircularArraysTest()
        {
            // Create a more complex circular structure
            var array1 = new Json5Array();
            var array2 = new Json5Array();
            var array3 = new Json5Array();

            array1.Add(array2);
            array2.Add(array3);
            array3.Add(array1); // Creates a cycle

            // This should throw Json5CircularReferenceException
            Assert.ThrowsExactly<Json5CircularReferenceException>(() => Json5.Stringify(array1));
        }
    }
}
