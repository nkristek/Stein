using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Stein.Views.Resources
{
    /// <inheritdoc />
    /// <summary>
    /// Wraps up a <see cref="T:System.Threading.Timer" /> with only a <see cref="T:System.WeakReference" /> to the callback so that the timer does not prevent GC from collecting the object that uses this timer.
    /// Your object must hold a reference to the callback passed into this timer.
    /// </summary>
    internal class WeakTimer : IDisposable
    {
        private readonly Timer _timer;

        private readonly WeakReference<TimerCallback> _weakCallback;

        public WeakTimer(TimerCallback callback)
        {
            _timer = new Timer(OnTimerCallback);
            _weakCallback = new WeakReference<TimerCallback>(callback);
        }

        public WeakTimer(TimerCallback callback, object state, int dueTime, int period)
        {
            _timer = new Timer(OnTimerCallback, state, dueTime, period);
            _weakCallback = new WeakReference<TimerCallback>(callback);
        }

        public WeakTimer(TimerCallback callback, object state, TimeSpan dueTime, TimeSpan period)
        {
            _timer = new Timer(OnTimerCallback, state, dueTime, period);
            _weakCallback = new WeakReference<TimerCallback>(callback);
        }

        public WeakTimer(TimerCallback callback, object state, uint dueTime, uint period)
        {
            _timer = new Timer(OnTimerCallback, state, dueTime, period);
            _weakCallback = new WeakReference<TimerCallback>(callback);
        }

        public WeakTimer(TimerCallback callback, object state, long dueTime, long period)
        {
            _timer = new Timer(OnTimerCallback, state, dueTime, period);
            _weakCallback = new WeakReference<TimerCallback>(callback);
        }

        private void OnTimerCallback(object state)
        {
            if (_weakCallback.TryGetTarget(out var callback))
                callback(state);
            else
                _timer.Dispose();
        }

        public bool Change(int dueTime, int period)
        {
            return _timer.Change(dueTime, period);
        }

        public bool Change(TimeSpan dueTime, TimeSpan period)
        {
            return _timer.Change(dueTime, period);
        }

        public bool Change(uint dueTime, uint period)
        {
            return _timer.Change(dueTime, period);
        }

        public bool Change(long dueTime, long period)
        {
            return _timer.Change(dueTime, period);
        }

        public bool Dispose(WaitHandle notifyObject)
        {
            return _timer.Dispose(notifyObject);
        }

        public void Dispose()
        {
            _timer.Dispose();
        }
    }
}
