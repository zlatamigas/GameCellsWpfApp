using QueenGame.WPF.Stores;

namespace QueenGame.WPF.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly NavigationStore navigationStore;

        public BaseViewModel? CurrentViewModel => navigationStore.CurrentViewModel;

        public MainWindowViewModel(NavigationStore navigationStore)
        {
            this.navigationStore = navigationStore;
            this.navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;            
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
