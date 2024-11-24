using QueenGame.WPF.ViewModels;

namespace QueenGame.WPF.Stores
{
    public class NavigationStore
    {
        private BaseViewModel? _CurrentViewModel;        
        public BaseViewModel? CurrentViewModel
        {
            get { return _CurrentViewModel; }
            set 
            {
                _CurrentViewModel?.Dispose();
                _CurrentViewModel = value;
                OnCurrentViewModelChanged();
            }
        
        }

        public event Action? CurrentViewModelChanged;

        private void OnCurrentViewModelChanged()
        {
            CurrentViewModelChanged?.Invoke();
        }
    }
}
