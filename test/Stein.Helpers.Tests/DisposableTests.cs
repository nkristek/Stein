﻿using System;
using System.Runtime.CompilerServices;
using Xunit;

namespace Stein.Helpers.Tests
{
    public class DisposableTests
    {
        private class DisposableImpl
            : Disposable
        {
            public Action OnDisposeManagedResources;

            public Action OnDisposeNativeResources;

            protected override void DisposeManagedResources()
            {
                base.DisposeManagedResources();

                OnDisposeManagedResources?.Invoke();
            }

            protected override void DisposeNativeResources()
            {
                base.DisposeNativeResources();

                OnDisposeNativeResources?.Invoke();
            }
        }

        [Fact]
        public void DisposeManagedResources_Dispose()
        {
            var managedResourcesDisposed = false;
            var instance = new DisposableImpl
            {
                OnDisposeManagedResources = () => managedResourcesDisposed = true
            };
            instance.Dispose();
            Assert.True(managedResourcesDisposed);
        }

        [Fact]
        public void DisposeNativeResourcesDisposed_Dispose()
        {
            var nativeResourcesDisposed = false;
            var instance = new DisposableImpl
            {
                OnDisposeNativeResources = () => nativeResourcesDisposed = true
            };
            instance.Dispose();
            Assert.True(nativeResourcesDisposed);
        }

        [Fact]
        public void DisposeManagedResources_Finalize()
        {
            var managedResourcesDisposed = false;
            CreateDisposableInstance(() => managedResourcesDisposed = true, null);
            GCHelper.TriggerGC();
            Assert.False(managedResourcesDisposed);
        }

        [Fact]
        public void DisposeNativeResourcesDisposed_Finalize()
        {
            var nativeResourcesDisposed = false;
            CreateDisposableInstance(null, () => nativeResourcesDisposed = true);
            GCHelper.TriggerGC();
            Assert.True(nativeResourcesDisposed);
        }

        [MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
        private static void CreateDisposableInstance(Action onDisposeManagedResources, Action onDisposeNativeResources)
        {
            var instance = new DisposableImpl
            {
                OnDisposeManagedResources = onDisposeManagedResources,
                OnDisposeNativeResources = onDisposeNativeResources
            };
        }
    }
}
