using QueenGame.WPF.Stores;
using QueenGame.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueenGame.WPF.Commands
{
    public class RestartCommand : BaseCommand
    {
        private LevelViewModel viewModel;
        private LevelStore levelStore;

        public RestartCommand(LevelViewModel viewModel, LevelStore levelStore)
        {
            this.viewModel = viewModel;
            this.levelStore = levelStore;
        }

        public override void Execute(object? parameter)
        {            
            levelStore.ResetGame();
            viewModel.OnCellsChanged();
        }
    }
}
