using Prism.Mvvm;

namespace WordQuiz.Models
{
    public class Word : BindableBase
    {

        public string Name { get; set; }     
        private bool _check;
        public bool Check
        {
            get { return _check; }
            set { SetProperty(ref _check, value); }
        }
    }
}