using System;

namespace WordQuiz.Services
{
    public interface IUITaskService
    {
        void RunOnUiTask(Action action);
    }
}