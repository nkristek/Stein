using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Stein.Helpers.Tests
{
    [TestClass]
    public class StructExtensionsTests
    {
        [TestMethod]
        public void TestIsDefault()
        {
            Assert.IsTrue(false.IsDefault());
            Assert.IsFalse(true.IsDefault());
        }
    }
}
