using System;

namespace WpfBase
{
    public class Disposable
        : IDisposable
    {
        /// <summary>
        /// Override to dispose managed resources.
        /// A managed resource is another managed type, which implements IDisposable.
        /// </summary>
        protected virtual void DisposeManagedResources() { }

        /// <summary>
        /// Override to dispose native resources.
        /// Native resources are anything outside the managed world such as native Windows handles etc.
        /// </summary>
        protected virtual void DisposeNativeResources() { }

        public void Dispose()
        {
            DisposeManagedResources();
            DisposeNativeResources();
            GC.SuppressFinalize(this);
        }

        ~Disposable()
        {
            DisposeNativeResources();
        }
    }
}
