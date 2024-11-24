using QueenGame.Services.GameInfoProvider;
using QueenGame.Services.GameProvider;
using QueenGame.WPF.Stores;
using QueenGame.WPF.ViewModels;

namespace QueenGame.WPF.Commands
{
    public class LoadLevelsInfoCommand : AsyncBaseCommand
    {
        private readonly LevelsListingViewModel viewModel;
        private readonly IGameInfoProvider gameInfoProvider;
        private readonly LevelsInfoStore levelsStore;

        public LoadLevelsInfoCommand(
            LevelsListingViewModel viewModel, 
            IGameInfoProvider gameInfoProvider, 
            LevelsInfoStore levelsStore,
            Action<Exception>? onExceptionAction) 
            : base(onExceptionAction)
        {
            this.viewModel = viewModel;
            this.gameInfoProvider = gameInfoProvider;
            this.levelsStore = levelsStore;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            viewModel.IsLoading = true;
            await levelsStore.Load();
            viewModel.UpdateGameLevels(levelsStore.GetGameInfos());
            viewModel.IsLoading = false;
        }
    }
}
