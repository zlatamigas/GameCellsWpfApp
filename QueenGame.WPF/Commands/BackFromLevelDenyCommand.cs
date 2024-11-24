using QueenGame.WPF.Services;
using QueenGame.WPF.Stores;
using QueenGame.WPF.ViewModels;
using System.ComponentModel;
using System.Windows;

namespace QueenGame.WPF.Commands
{
    public class BackFromLevelDenyCommand: BaseCommand 
    {
        private LevelViewModel viewModel;

        public BackFromLevelDenyCommand(
            LevelViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override void Execute(object? parameter)
        {
            viewModel.IsRequestedBack = false;          
        }
    }
}
