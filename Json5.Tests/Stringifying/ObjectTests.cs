using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Json5.Tests.Stringifying
{
    /// <summary>
    /// Contains unit tests for JSON5 object stringification.
    /// </summary>
    [TestClass]
    public class ObjectTests
    {
        /// <summary>
        /// Tests stringification of empty objects.
        /// </summary>
        [TestMethod]
        public void EmptyObjectsTest()
        {
            var s = Json5.Stringify(new Json5Object());
            Assert.AreEqual("{}", s, "Expected the stringified value to be '{}' for an empty object.");
        }

        /// <summary>
        /// Tests stringification of objects with unquoted property names.
        /// </summary>
        [TestMethod]
        public void UnquotedPropertyNamesTest()
        {
            var s = Json5.Stringify(new Json5Object { { "a", 1 } });
            Assert.AreEqual("{a:1}", s, "Expected the stringified value to be '{a:1}' for an object with unquoted property names.");
        }

        /// <summary>
        /// Tests stringification of objects with single-quoted string property names.
        /// </summary>
        [TestMethod]
        public void SingleQuotedStringPropertyNamesTest()
        {
            var s = Json5.Stringify(new Json5Object { { "a-b", 1 } });
            Assert.AreEqual("{'a-b':1}", s, "Expected the stringified value to be '{'a-b':1}' for an object with single-quoted string property names.");
        }

        /// <summary>
        /// Tests stringification of objects with double-quoted string property names.
        /// </summary>
        [TestMethod]
        public void DoubleQuotedStringPropertyNamesTest()
        {
            var s = Json5.Stringify(new Json5Object { { "a'", 1 } });
            Assert.AreEqual("{\"a'\":1}", s, "Expected the stringified value to be '{\"a'\":1}' for an object with double-quoted string property names.");
        }

        /// <summary>
        /// Tests stringification of objects with empty string property names.
        /// </summary>
        [TestMethod]
        public void EmptyStringPropertyNamesTest()
        {
            var s = Json5.Stringify(new Json5Object { { "", 1 } });
            Assert.AreEqual("{'':1}", s, "Expected the stringified value to be '{'':1}' for an object with empty string property names.");
        }

        /// <summary>
        /// Tests stringification of objects with special character property names.
        /// </summary>
        [TestMethod]
        public void SpecialCharacterPropertyNamesTest()
        {
            var s = Json5.Stringify(new Json5Object { { "$_", 1 }, { "_$", 2 }, { "a\u200C", 3 } });
            Assert.AreEqual("{$_:1,_$:2,a\u200C:3}", s, "Expected the stringified value to be '{$_:1,_$:2,a\u200C:3}' for an object with special character property names.");
        }

        /// <summary>
        /// Tests stringification of objects with Unicode property names.
        /// </summary>
        [TestMethod]
        public void UnicodePropertyNamesTest()
        {
            var s = Json5.Stringify(new Json5Object { { "ùńîċõďë", 9 } });
            Assert.AreEqual("{ùńîċõďë:9}", s, "Expected the stringified value to be '{ùńîċõďë:9}' for an object with Unicode property names.");
        }

        /// <summary>
        /// Tests stringification of objects with escaped property names.
        /// </summary>
        [TestMethod]
        public void EscapedPropertyNamesTest()
        {
            var s = Json5.Stringify(new Json5Object { { "\\\b\f\n\r\t\v\0\x01", 1 } });
            Assert.AreEqual(@"{'\\\b\f\n\r\t\v\0\x01':1}", s, "Expected the stringified value to be '{'\\\b\f\n\r\t\v\0\x01':1}' for an object with escaped property names.");
        }

        /// <summary>
        /// Tests stringification of objects with multiple properties.
        /// </summary>
        [TestMethod]
        public void MultiplePropertiesTest()
        {
            var s = Json5.Stringify(new Json5Object { { "abc", 1 }, { "def", 2 } });
            Assert.AreEqual("{abc:1,def:2}", s, "Expected the stringified value to be '{abc:1,def:2}' for an object with multiple properties.");
        }

        /// <summary>
        /// Tests stringification of nested objects.
        /// </summary>
        [TestMethod]
        public void NestedObjectsTest()
        {
            var s = Json5.Stringify(new Json5Object { { "a", new Json5Object { { "b", 2 } } } });
            Assert.AreEqual("{a:{b:2}}", s, "Expected the stringified value to be '{a:{b:2}}' for nested objects.");
        }

        /// <summary>
        /// Tests stringification of circular objects.
        /// </summary>
        [TestMethod]
        public void CircularObjectsTest()
        {
            var o = new Json5Object();
            o["a"] = o;
            //Json5.Stringify(o);
            Assert.Fail("Not Implemented", "Expected the test to fail as circular object stringification is not implemented.");
        }
    }
}
