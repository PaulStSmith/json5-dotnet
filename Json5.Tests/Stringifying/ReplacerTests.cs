using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Json5.Tests.Stringifying
{
    /// <summary>
    /// Contains unit tests for JSON5 replacer function.
    /// </summary>
    [TestClass]
    public class ReplacerTests
    {
        /// <summary>
        /// Tests stringification with a list of keys to include.
        /// </summary>
        [TestMethod]
        public void KeysTest()
        {
            var s = Json5.Stringify(new Json5Object { { "a", 1 }, { "b", 2 }, { "3", 3 } }, ["a", "3"]);
            Assert.AreEqual("{a:1,'3':3}", s, "Expected the stringified value to include only the specified keys 'a' and '3'.");
        }

        /// <summary>
        /// Tests stringification with a replacer function.
        /// </summary>
        [TestMethod]
        public void FunctionTest()
        {
            var s = Json5.Stringify(new Json5Object { { "a", 1 }, { "b", 2 } }, (k, v) => (k == "a") ? 2 : v);
            Assert.AreEqual("{a:2,b:2}", s, "Expected the stringified value to have the property 'a' modified to 2 by the replacer function.");
        }

        /// <summary>
        /// Tests exposing the parent value using a replacer function.
        /// </summary>
        [TestMethod]
        public void ExposesParentValueTest()
        {
            var s = Json5.Stringify(Json5.Parse("{a:{b:1}}"), (p, k, v) => (k == "b" && (double?)p["b"] != null) ? 2 : v);
            Assert.AreEqual("{a:{b:2}}", s, "Expected the stringified value to have the nested property 'b' modified to 2 when the parent value is exposed.");
        }
    }
}
