using System;
using System.Threading;
using Xamarin.Forms;

namespace WordQuiz.Services
{
    public class TimerService : ITimerService
    {
        private CancellationTokenSource _cancellation;

        public TimerService()
        {
            _cancellation = new CancellationTokenSource();
        }

        public void Start(TimeSpan timespan, Action callback)
        {
            CancellationTokenSource cts = _cancellation; // safe copy
            Device.StartTimer(timespan,
                () =>
                {
                    if (cts.IsCancellationRequested) return false;
                    callback.Invoke();
                    return true; // or true for periodic behavior
                });
        }

        public void Stop()
        {
            Interlocked.Exchange(ref _cancellation, new CancellationTokenSource()).Cancel();
        }
    }
}
