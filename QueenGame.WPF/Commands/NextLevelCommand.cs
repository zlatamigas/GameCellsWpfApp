using QueenGame.WPF.Services;
using QueenGame.WPF.Stores;
using QueenGame.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueenGame.WPF.Commands
{
    public class NextLevelCommand : BaseCommand
    {
        private readonly LevelViewModel viewModel;
        private readonly LevelStore levelStore;
        private readonly LevelsInfoStore levelsInfoStore;
        private readonly NavigationService<LevelViewModel> navigationService;

        public NextLevelCommand(
            LevelViewModel viewModel,
            LevelStore levelStore,
            LevelsInfoStore levelsInfoStore,
            NavigationService<LevelViewModel> navigationService)
        {
            this.viewModel = viewModel;
            this.levelStore = levelStore;
            this.levelsInfoStore = levelsInfoStore;
            this.navigationService = navigationService;
        }

        public override void Execute(object? parameter)
        {
            if (viewModel.GameId != null)
            {
                levelStore.CurrentLevelOption = levelsInfoStore.GetNextLevelOption(viewModel.GameId.Value);
            }
            else if (viewModel.Size != null)
            {
                levelStore.CurrentLevelOption = new LevelOption
                {
                    GameId = null,
                    Size = viewModel.Size.Value
                };
            }
            else 
            { 
                return;
            }

            navigationService.Navigate();
        }
    }
}
