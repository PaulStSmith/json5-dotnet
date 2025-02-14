using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Json5.Tests.Parsing
{
    /// <summary>
    /// Contains unit tests for JSON5 object parsing.
    /// </summary>
    [TestClass]
    public class ObjectTests
    {
        /// <summary>
        /// Tests parsing of empty objects.
        /// </summary>
        [TestMethod]
        public void EmptyObjectsTest()
        {
            var v = Json5.Parse("{}");
            var o = (Json5Object)v;
            Assert.AreEqual(0, o.Count);
        }

        /// <summary>
        /// Tests parsing of objects with double-quoted property names.
        /// </summary>
        [TestMethod]
        public void DoubleStringPropertyNamesTest()
        {
            var v = Json5.Parse("{\"a\":1}");
            var o = (Json5Object)v;
            Assert.AreEqual(1, o.Count);
            Assert.AreEqual(1D, (double)o["a"]);
        }

        /// <summary>
        /// Tests parsing of objects with single-quoted property names.
        /// </summary>
        [TestMethod]
        public void SingleStringPropertyNamesTest()
        {
            var v = Json5.Parse("{'a':1}");
            var o = (Json5Object)v;
            Assert.AreEqual(1, o.Count);
            Assert.AreEqual(1D, (double)o["a"]);
        }

        /// <summary>
        /// Tests parsing of objects with unquoted property names.
        /// </summary>
        [TestMethod]
        public void UnquotedPropertyNamesTest()
        {
            var v = Json5.Parse("{a:1}");
            var o = (Json5Object)v;
            Assert.AreEqual(1, o.Count);
            Assert.AreEqual(1D, (double)o["a"]);
        }

        /// <summary>
        /// Tests parsing of objects with special character property names.
        /// </summary>
        [TestMethod]
        public void SpecialCharacterPropertyNamesTest()
        {
            var v = Json5.Parse("{$_:1,_$:2,a\u200C:3}");
            var o = (Json5Object)v;
            Assert.AreEqual(3, o.Count);
            Assert.AreEqual(1D, (double)o["$_"]);
            Assert.AreEqual(2D, (double)o["_$"]);
            Assert.AreEqual(3D, (double)o["a\u200C"]);
        }

        /// <summary>
        /// Tests parsing of objects with Unicode property names.
        /// </summary>
        [TestMethod]
        public void UnicodePropertyNamesTest()
        {
            var v = Json5.Parse("{ùńîċõďë:9}");
            var o = (Json5Object)v;
            Assert.AreEqual(1, o.Count);
            Assert.AreEqual(9D, (double)o["ùńîċõďë"]);
        }

        /// <summary>
        /// Tests parsing of objects with escaped property names.
        /// </summary>
        [TestMethod]
        public void EscapedPropertyNamesTest()
        {
            var v = Json5.Parse(@"{\u0061\u0062:1,\u0024\u005F:2,\u005F\u0024:3}");
            var o = (Json5Object)v;
            Assert.AreEqual(3, o.Count);
            Assert.AreEqual(1D, (double)o["ab"]);
            Assert.AreEqual(2D, (double)o["$_"]);
            Assert.AreEqual(3D, (double)o["_$"]);
        }

        /// <summary>
        /// Tests parsing of objects with multiple properties.
        /// </summary>
        [TestMethod]
        public void MultiplePropertiesTest()
        {
            var v = Json5.Parse("{abc:1,def:2}");
            var o = (Json5Object)v;
            Assert.AreEqual(2, o.Count);
            Assert.AreEqual(1D, (double)o["abc"]);
            Assert.AreEqual(2D, (double)o["def"]);
        }

        /// <summary>
        /// Tests parsing of nested objects.
        /// </summary>
        [TestMethod]
        public void NestedObjectsTest()
        {
            var v = Json5.Parse("{a:{b:2}}");
            var o = (Json5Object)v;
            Assert.AreEqual(1, o.Count);
            Assert.AreEqual(2D, (double)o["a"]["b"]);
        }
    }
}
