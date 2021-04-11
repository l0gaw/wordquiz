using Prism.Mvvm;
using Prism.Navigation;
using System.Threading.Tasks;

namespace WordQuiz.ViewModels
{
    public class ViewModelBase : BindableBase, IInitializeAsync, INavigationAware, IDestructible
    {
        protected INavigationService NavigationService { get; private set; }         

        public ViewModelBase(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }       

        public virtual Task InitializeAsync(INavigationParameters parameters)
        {
            return Task.CompletedTask;
        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {

        }

        public virtual void Destroy()
        {

        }

    }
}
