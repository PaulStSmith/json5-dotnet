using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

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
            // Create an object that contains itself
            var obj = new Json5Object();
            obj["self"] = obj;

            // This should throw Json5CircularReferenceException
            Assert.ThrowsExactly<Json5CircularReferenceException>(() => Json5.Stringify(obj));
        }

        /// <summary>
        /// Tests stringification of deep circular objects.
        /// </summary>
        [TestMethod]
        public void DeepCircularObjectsTest()
        {
            // Create a more complex circular structure
            var obj1 = new Json5Object();
            var obj2 = new Json5Object();
            var obj3 = new Json5Object();

            obj1["next"] = obj2;
            obj2["next"] = obj3;
            obj3["next"] = obj1; // Creates a cycle

            // This should throw Json5CircularReferenceException
            Assert.ThrowsExactly<Json5CircularReferenceException>(() => Json5.Stringify(obj1));
        }

        /// <summary>
        /// Tests stringification of non-circular complex structures.
        /// </summary>
        [TestMethod]
        public void NonCircularComplexStructureTest()
        {
            // Create a complex but non-circular structure
            var obj1 = new Json5Object();
            var obj2 = new Json5Object();
            var array1 = new Json5Array();
            var array2 = new Json5Array();

            obj1["array"] = array1;
            array1.Add(obj2);
            obj2["array"] = array2;
            array2.Add(42); // No cycle created

            // This should succeed
            var result = Json5.Stringify(obj1);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Contains("42")); // Verify the deep structure was stringified
        }

        /// <summary>
        /// Tests stringification of objects with multiple references to the same object, but no cycles.
        /// </summary>
        [TestMethod]
        public void MultipleReferencesNonCircularTest()
        {
            // Create a structure with multiple references to the same object, but no cycles
            var sharedObj = new Json5Object { ["value"] = 42 };
            var container = new Json5Object
            {
                ["ref1"] = sharedObj,
                ["ref2"] = sharedObj
            };

            // This should succeed - multiple references are allowed, just not circular ones
            var result = Json5.Stringify(container);
            Assert.IsNotNull(result);

            // The value 42 should appear twice in the output
            var count = result.Split(["42"], StringSplitOptions.None).Length - 1;
            Assert.AreEqual(2, count, "The shared value should appear twice in the stringified output");
        }
    }
}
