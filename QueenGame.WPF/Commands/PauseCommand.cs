using QueenGame.WPF.Stores;
using QueenGame.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueenGame.WPF.Commands
{
    public class PauseCommand : BaseCommand
    {
        private LevelViewModel viewModel;
        private LevelStore levelStore;

        public PauseCommand(LevelViewModel viewModel, LevelStore levelStore)
        {
            this.viewModel = viewModel;
            this.levelStore = levelStore;

            viewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
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
            viewModel.PauseClickedEvent?.Invoke(viewModel, new System.Windows.RoutedEventArgs());
        }

        public override bool CanExecute(object? parameter)
        {
            return base.CanExecute(parameter)
                && viewModel.LevelState == LevelState.STARTED;
        }
    }
}
