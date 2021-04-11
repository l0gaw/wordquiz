using Moq;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WordQuiz.Models;
using WordQuiz.Services;
using WordQuiz.ViewModels;
using Xunit;

namespace WordQuiz.Tests
{
    public class MainViewModelTests
    {
        private readonly Mock<INavigationService> _navigationService;
        private readonly Mock<ITimerService> _timerService;
        private readonly Mock<IUITaskService> _uiTaskService;
        private readonly Mock<IWordService> _wordService;
        private readonly MainPageViewModel _mainViewModel;

        public MainViewModelTests()
        {
            _navigationService = new Mock<INavigationService>();
            _timerService = new Mock<ITimerService>();
            _uiTaskService = new Mock<IUITaskService>();
            _wordService = new Mock<IWordService>();
            _mainViewModel = new MainPageViewModel(_navigationService.Object, _timerService.Object, _uiTaskService.Object, _wordService.Object);
        }

        [Fact]
        public void SetSearch_Should_Call_MatchWord()
        {
            _mainViewModel.Words = GetWords();
            _mainViewModel.Search = "teste2";
            var foundWord = _mainViewModel.Words.First(w => w.Name.Equals(_mainViewModel.Search));
            Assert.True(foundWord.Check);
        }

        [Fact]
        public void MatchWord_Should_Check_The_Found_Word_And_Set_Score()
        {
            _mainViewModel.Words = GetWords();
            var search = "teste2";
            _mainViewModel.MatchWord(search);
            var wordFound = _mainViewModel.Words.First(w => w.Name.Equals(search));
            Assert.True(wordFound.Check);
            Assert.Equal(1, _mainViewModel.Score);
        }

        [Fact]
        public void StartQuiz_Should_Call_Start_And_Set_IsTimeRunning_True()
        {
            _timerService.Setup(t => t.Start(TimeSpan.FromSeconds(1), It.IsAny<Action>()));

            _mainViewModel.StartQuiz();

            _timerService.Verify(t => t.Start(TimeSpan.FromSeconds(1), It.IsAny<Action>()));
            Assert.True(_mainViewModel.IsTimeRunning);
        }

        [Fact]
        public void DecreaseTime_Should_Decrease_Time_One_Second()
        {
            var originalTime = TimeSpan.FromMinutes(5);

            _mainViewModel.DecreaseTime();

            Assert.Equal(originalTime.Subtract(TimeSpan.FromSeconds(1)), _mainViewModel.Time);
        }

        [Fact]
        public void DecreaseTime_Should_Stop_Timer_When_Time_Is_Zero()
        {
            _mainViewModel.Time = TimeSpan.FromSeconds(1);
            _timerService.Setup(t => t.Stop());
            _mainViewModel.DecreaseTime();
            _timerService.Verify(t => t.Stop());
        }

        [Fact]
        public void Reset_Should_Stop_Timer_And_Reset_All_The_Things()
        {
            _timerService.Setup(t => t.Stop());
            _mainViewModel.Time = TimeSpan.FromMinutes(3);
            _mainViewModel.Words = GetWords();
            _mainViewModel.StartQuiz();
            _mainViewModel.Search = "teste2";

            _mainViewModel.ResetGame();

            _timerService.Verify(t => t.Stop());
            Assert.Equal(TimeSpan.FromMinutes(5), _mainViewModel.Time);
            Assert.False(_mainViewModel.IsTimeRunning);
            Assert.Equal(0, _mainViewModel.Score);
            Assert.Equal(string.Empty, _mainViewModel.Search);
            Assert.All(_mainViewModel.Words, w => Assert.False(w.Check));
        }

        [Fact]
        public async Task InitializeAsync_Should_Call_WordService_GetWords()
        {
            _wordService.Setup(t => t.GetWords()).Returns(() => Task.FromResult(GetWords().AsEnumerable()));

            await _mainViewModel.InitializeAsync(new NavigationParameters());

            Assert.Equal(_mainViewModel.Words.Count(), GetWords().Count());
        }

        private ObservableCollection<Word> GetWords()
        {
            return new ObservableCollection<Word>() { new Word { Name = "teste" }, new Word { Name = "teste2" } };
        }
    }
}
