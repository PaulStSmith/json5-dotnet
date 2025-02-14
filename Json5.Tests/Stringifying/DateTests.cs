using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Json5.Tests.Stringifying
{
    /// <summary>
    /// Contains unit tests for JSON5 date stringification.
    /// </summary>
    [TestClass]
    public class DateTests
    {
        /// <summary>
        /// Tests stringification of date values.
        /// </summary>
        [TestMethod]
        public void DatesTest()
        {
            var s = Json5.Stringify(new DateTime(2016, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            Assert.AreEqual("'2016-01-01T00:00:00.000Z'", s, "Expected the stringified value to be '2016-01-01T00:00:00.000Z' for the given DateTime value.");
        }
    }
}
