using QueenGame.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueenGame.WPF.Commands
{
    public class ShowLangsCommand : BaseCommand
    {
        private readonly SettingsViewModel viewModel;

        public ShowLangsCommand(SettingsViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override void Execute(object? parameter)
        {
            viewModel.LangsShown = !viewModel.LangsShown;
        }
    }
}
