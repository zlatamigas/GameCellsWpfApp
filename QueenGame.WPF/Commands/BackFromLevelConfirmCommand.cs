using QueenGame.WPF.Services;
using QueenGame.WPF.Stores;
using QueenGame.WPF.ViewModels;
using System.ComponentModel;
using System.Windows;

namespace QueenGame.WPF.Commands
{
    public class BackFromLevelConfirmCommand<TViewModel> : BaseCommand where TViewModel : BaseViewModel
    {
        private LevelViewModel viewModel;
        private LevelStore levelStore;
        private NavigationService<TViewModel> navigationService;
        private readonly bool requireConfirm;

        public BackFromLevelConfirmCommand(
            LevelViewModel viewModel,
            LevelStore levelStore,
            NavigationService<TViewModel> navigationService)
        {
            this.viewModel = viewModel;
            this.levelStore = levelStore;
            this.navigationService = navigationService;

            viewModel.PropertyChanged += OnLevelViewModelPropertyChanged;
        }

        private void OnLevelViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(LevelViewModel.LevelState):
                    OnCanExecuteChanged();
                    break;
            }
        }

        public override void Execute(object? parameter)
        {
            levelStore.ClearGame();
            viewModel.IsRequestedBack = false;
            navigationService?.Navigate();             
        }

        public override bool CanExecute(object? parameter)
        {
            return base.CanExecute(parameter)
                && levelStore.CurrentLevelState != LevelState.LOADING;
        }
    }
}
