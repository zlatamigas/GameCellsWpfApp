using QueenGame.Services.GameInfoProvider;
using QueenGame.Services.GameProvider;
using QueenGame.Services.GeneratedGameResultProvider;
using QueenGame.WPF.Stores;
using QueenGame.WPF.ViewModels;

namespace QueenGame.WPF.Commands
{
    public class LoadGeneratedGameResultsCommand : AsyncBaseCommand
    {
        private readonly GenerateLevelOptionsViewModel viewModel;
        private readonly IGeneratedGameResultProvider generatedGameResultProvider;

        public LoadGeneratedGameResultsCommand(
            GenerateLevelOptionsViewModel viewModel, 
            IGeneratedGameResultProvider generatedGameResultProvider, 
            Action<Exception>? onExceptionAction) 
            : base(onExceptionAction)
        {
            this.viewModel = viewModel;
            this.generatedGameResultProvider = generatedGameResultProvider;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            viewModel.IsLoading = true;
            viewModel.UpdateGeneratedGameResults(await generatedGameResultProvider.GetGeneratedGameResults());
            viewModel.IsLoading = false;
        }
    }
}
