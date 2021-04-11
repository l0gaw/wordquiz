using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WordQuiz.Models;
using WordQuiz.Services;

namespace WordQuiz.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly ITimerService _timeService;
        private readonly IUITaskService _uiTaskService;
        private readonly IWordService _wordService;
        private ObservableCollection<Word> words;
        public ObservableCollection<Word> Words { get => words; set => SetProperty(ref words, value); }

        private string search;
        public string Search
        {
            get => search; set
            {
                SetProperty(ref search, value);
                MatchWord(search);
            }
        }

        private int score;
        public int Score { get => score; set => SetProperty(ref score, value); }

        private TimeSpan time;
        public TimeSpan Time { get => time; set => SetProperty(ref time, value); }

        private bool isTimeRunning;
        public bool IsTimeRunning { get => isTimeRunning; set => SetProperty(ref isTimeRunning, value); }

        public DelegateCommand PlayQuizCommand { get; }
        public DelegateCommand StopQuizCommand { get; }

        public MainPageViewModel(INavigationService navigationService,
                                 ITimerService timeService,
                                 IUITaskService uiTaskService,
                                 IWordService wordService)
            : base(navigationService)
        {
            _timeService = timeService;
            _uiTaskService = uiTaskService;
            _wordService = wordService;
            PlayQuizCommand = new DelegateCommand(StartQuiz);
            StopQuizCommand = new DelegateCommand(ResetGame);
            Time = TimeSpan.FromMinutes(5);
        }

        public void MatchWord(string search)
        {
            var word = Words.FirstOrDefault(p => p.Name.Equals(search, StringComparison.OrdinalIgnoreCase));
            if (word != null && !word.Check)
            {
                word.Check = true;
                Score += 1;
            }
        }

        public void StartQuiz()
        {
            _timeService.Start(TimeSpan.FromSeconds(1), () => _uiTaskService.RunOnUiTask(DecreaseTime));
            IsTimeRunning = true;
        }

        public void DecreaseTime()
        {
            Time = Time.Subtract(TimeSpan.FromSeconds(1));

            if (Time.TotalSeconds == 0)
                _timeService.Stop();
        }

        public void ResetGame()
        {
            _timeService.Stop();
            Time = TimeSpan.FromMinutes(5);
            IsTimeRunning = false;
            Score = 0;
            Search = "";
            foreach (var word in Words)
            {
                word.Check = false;
            }
        }

        public override async Task InitializeAsync(INavigationParameters parameters)
        {
            Words = new ObservableCollection<Word>(await _wordService.GetWords());
        }
    }
}
