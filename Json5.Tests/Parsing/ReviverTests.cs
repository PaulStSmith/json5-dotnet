using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Json5.Tests.Parsing
{
    /// <summary>
    /// Contains unit tests for JSON5 reviver function.
    /// </summary>
    [TestClass]
    public class ReviverTests
    {
        /// <summary>
        /// Tests modifying property values using a reviver function.
        /// </summary>
        [TestMethod]
        public void ModifiesPropertyValuesTest()
        {
            var val = Json5.Parse("{a:1,b:2}", (k, v) => (k == "a") ? "revived" : v);
            Assert.AreEqual("revived", (string)val["a"], "Expected the property 'a' to be modified to 'revived'.");
            Assert.AreEqual(2D, (double)val["b"], "Expected the property 'b' to remain 2.");
        }

        /// <summary>
        /// Tests modifying nested object property values using a reviver function.
        /// </summary>
        [TestMethod]
        public void ModifiesNestedObjectPropertyValuesTest()
        {
            var val = Json5.Parse("{a:{b:2}}", (k, v) => (k == "b") ? "revived" : v);
            Assert.AreEqual("revived", (string)val["a"]["b"], "Expected the nested property 'b' to be modified to 'revived'.");
        }

        /// <summary>
        /// Tests deleting property values using a reviver function.
        /// </summary>
        [TestMethod]
        public void DeletesPropertyValuesTest()
        {
            var val = Json5.Parse("{a:1,b:2}", (k, v) => (k == "a") ? null : v);
            var o = (Json5Object)val;
            Assert.AreEqual(1, o.Count, "Expected the object to have one property after deleting 'a'.");
            Assert.AreEqual(2D, (double)o["b"], "Expected the property 'b' to remain 2.");
        }

        /// <summary>
        /// Tests modifying array values using a reviver function.
        /// </summary>
        [TestMethod]
        public void ModifiesArrayValuesTest()
        {
            var val = Json5.Parse("[0,1,2]", (k, v) => (k == "1") ? "revived" : v);
            Assert.AreEqual(0D, (double)val[0], "Expected the first array element to be 0.");
            Assert.AreEqual("revived", (string)val[1], "Expected the second array element to be modified to 'revived'.");
            Assert.AreEqual(2D, (double)val[2], "Expected the third array element to remain 2.");
        }

        /// <summary>
        /// Tests modifying nested array values using a reviver function.
        /// </summary>
        [TestMethod]
        public void ModifiesNestedArrayValuesTest()
        {
            var val = Json5.Parse("[0,[1,2,3]]", (k, v) => (k == "2") ? "revived" : v);
            Assert.AreEqual(0D, (double)val[0], "Expected the first array element to be 0.");
            Assert.AreEqual(1D, (double)val[1][0], "Expected the first nested array element to be 1.");
            Assert.AreEqual(2D, (double)val[1][1], "Expected the second nested array element to be 2.");
            Assert.AreEqual("revived", (string)val[1][2], "Expected the third nested array element to be modified to 'revived'.");
        }

        /// <summary>
        /// Tests deleting array values using a reviver function.
        /// </summary>
        [TestMethod]
        public void DeletesArrayValuesTest()
        {
            var val = Json5.Parse("[0,1,2]", (k, v) => (k == "1") ? null : v);
            Assert.AreEqual(0D, (double)val[0], "Expected the first array element to be 0.");
            Assert.IsNull(val[1], "Expected the second array element to be deleted.");
            Assert.AreEqual(2D, (double)val[2], "Expected the third array element to be 2.");
        }

        /// <summary>
        /// Tests modifying the root value using a reviver function.
        /// </summary>
        [TestMethod]
        public void ModifiesRootValueTest()
        {
            var val = Json5.Parse("1", (k, v) => (k == "") ? "revived" : v);
            Assert.AreEqual("revived", (string)val, "Expected the root value to be modified to 'revived'.");
        }

        /// <summary>
        /// Tests exposing the parent value using a reviver function.
        /// </summary>
        [TestMethod]
        public void ExposesParentValueTest()
        {
            var val = Json5.Parse("{a:{b:2}}", (p, k, v) => (k == "b" && (double?)p["b"] != null) ? "revived" : v);
            Assert.AreEqual("revived", (string)val["a"]["b"], "Expected the nested property 'b' to be modified to 'revived' when the parent value is exposed.");
        }
    }
}
