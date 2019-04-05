using Xunit;

namespace Stein.Helpers.Tests
{
    public class StructExtensionsTests
    {
        [Fact]
        public void IsDefault()
        {
            Assert.True(false.IsDefault());
            Assert.False(true.IsDefault());
        }
    }
}
