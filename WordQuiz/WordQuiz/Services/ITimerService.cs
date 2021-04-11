using System;

namespace WordQuiz.Services
{
    public interface ITimerService
    {
        void Start(TimeSpan timespan, Action callback);
        void Stop();
    }
}