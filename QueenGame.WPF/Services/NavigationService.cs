using QueenGame.WPF.Stores;
using QueenGame.WPF.ViewModels;

namespace QueenGame.WPF.Services
{
    public class NavigationService<TViewModel> where TViewModel : BaseViewModel
    {
        private readonly NavigationStore navigationStore;
        private readonly Func<TViewModel> viewModelCreateFunc;

        public NavigationService(NavigationStore navigationStore, Func<TViewModel> viewModelCreateFunc)
        {
            this.navigationStore = navigationStore;
            this.viewModelCreateFunc = viewModelCreateFunc;
        }

        public void Navigate() 
        { 
            navigationStore.CurrentViewModel = viewModelCreateFunc();
        }
    }
}
