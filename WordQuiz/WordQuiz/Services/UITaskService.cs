using System;
using Xamarin.Forms;

namespace WordQuiz.Services
{
    public class UITaskService : IUITaskService
    {
        public void RunOnUiTask(Action action)
        {
            Device.BeginInvokeOnMainThread(action);
        }
    }
}