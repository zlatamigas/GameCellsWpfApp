using QueenGame.WPF.Services;
using QueenGame.WPF.Stores;
using QueenGame.WPF.ViewModels;
using System.ComponentModel;
using System.Drawing;

namespace QueenGame.WPF.Commands
{
    public class GenerateLevelCommand : BaseCommand
    {
        private readonly GenerateLevelOptionsViewModel viewModel;

        private readonly NavigationService<LevelViewModel> navigationServiceLevelViewModel;
        private readonly LevelStore levelStore;

        public GenerateLevelCommand( 
            GenerateLevelOptionsViewModel viewModel,
            NavigationService<LevelViewModel> navigationServiceLevelViewModel,
            LevelStore levelStore)
        {
            this.viewModel = viewModel;
            this.navigationServiceLevelViewModel = navigationServiceLevelViewModel;
            this.levelStore = levelStore;

            viewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(GenerateLevelOptionsViewModel.Size):
                    OnCanExecuteChanged();
                    break;
            }
        }

        public override void Execute(object? parameter)
        {
            levelStore.CurrentLevelOption = new LevelOption() 
            { 
                GameId = null,
                Size = Int32.Parse(viewModel.Size),
            };

            navigationServiceLevelViewModel.Navigate();
        }

        public override bool CanExecute(object? parameter)
        {
            return base.CanExecute(parameter)
                && !viewModel.GetErrors(nameof(GenerateLevelOptionsViewModel.Size)).Cast<string>().Any();
        }
    }
}
