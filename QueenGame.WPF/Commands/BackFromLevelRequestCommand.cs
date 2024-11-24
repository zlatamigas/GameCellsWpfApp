using QueenGame.WPF.Services;
using QueenGame.WPF.Stores;
using QueenGame.WPF.ViewModels;
using System.ComponentModel;
using System.Windows;

namespace QueenGame.WPF.Commands
{
    public class BackFromLevelRequestCommand: BaseCommand
    {
        private LevelViewModel viewModel;
        private LevelStore levelStore;

        public BackFromLevelRequestCommand(
            LevelViewModel viewModel,
            LevelStore levelStore)
        {
            this.viewModel = viewModel;
            this.levelStore = levelStore;

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
                levelStore.PauseGame();
                viewModel.IsRequestedBack = true;
        }

        public override bool CanExecute(object? parameter)
        {
            return base.CanExecute(parameter)
                && levelStore.CurrentLevelState != LevelState.LOADING;
        }
    }
}
