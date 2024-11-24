using QueenGame.WPF.Services;
using QueenGame.WPF.Stores;
using QueenGame.WPF.ViewModels;

namespace QueenGame.WPF.Commands
{
    public class ProvideLevelCommand : AsyncBaseCommand
    {
        private readonly LevelOption levelOption;
        private readonly NavigationService<LevelViewModel> navigationServiceLevelViewModel;
        private readonly LevelStore levelStore;

        public ProvideLevelCommand(
            LevelOption levelOption,
            NavigationService<LevelViewModel> navigationServiceLevelViewModel,
            LevelStore levelStore)
        {
            this.levelOption = levelOption;
            this.navigationServiceLevelViewModel = navigationServiceLevelViewModel;
            this.levelStore = levelStore;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            levelStore.CurrentLevelOption = new LevelOption() 
            { 
                GameId = levelOption.GameId,
                Size = null,
            };

            navigationServiceLevelViewModel.Navigate();
        }
    }
}
