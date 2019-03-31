using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Stein.Helpers.Tests
{
    [TestClass]
    public class ActionDisposableTests
    {
        [TestMethod]
        public void Constructor_no_actions_no_exceptions()
        {
            var instance = new ActionDisposable(null);
            instance.Dispose();
        }

        [TestMethod]
        public void DisposeManagedResources_Dispose()
        {
            var managedResourcesDisposed = false;
            var instance = new ActionDisposable(() => managedResourcesDisposed = true);
            instance.Dispose();
            Assert.IsTrue(managedResourcesDisposed);
        }

        [TestMethod]
        public void DisposeNativeResourcesDisposed_Dispose()
        {
            var nativeResourcesDisposed = false;
            var instance = new ActionDisposable(null, () => nativeResourcesDisposed = true);
            instance.Dispose();
            Assert.IsTrue(nativeResourcesDisposed);
        }
    }
}
