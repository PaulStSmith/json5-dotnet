using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Json5.Tests.Parsing
{
    /// <summary>
    /// Contains unit tests for JSON5 comment parsing.
    /// </summary>
    [TestClass]
    public class CommentTests
    {
        /// <summary>
        /// Tests parsing of single-line comments.
        /// </summary>
        [TestMethod]
        public void SingleLineCommentsTest()
        {
            var v = Json5.Parse("{//comment\n}");
            var o = (Json5Object)v;
            Assert.AreEqual(0, o.Count);
        }

        /// <summary>
        /// Tests parsing of single-line comments at the end of the file.
        /// </summary>
        [TestMethod]
        public void SingleLineCommentsAtEofTest()
        {
            var v = Json5.Parse("{}//comment");
            var o = (Json5Object)v;
            Assert.AreEqual(0, o.Count);
        }

        /// <summary>
        /// Tests parsing of multi-line comments.
        /// </summary>
        [TestMethod]
        public void MultiLineComments()
        {
            var v = Json5.Parse("{/*comment\n** */}");
            var o = (Json5Object)v;
            Assert.AreEqual(0, o.Count);
        }
    }
}
