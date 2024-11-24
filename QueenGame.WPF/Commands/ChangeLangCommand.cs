using QueenGame.WPF.Stores;
using QueenGame.WPF.ViewModels.ObjectViewModels;
using QueenGame.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueenGame.WPF.Commands
{
    public class ChangeLangCommand : BaseCommand
    {
        private readonly SettingsViewModel settingsViewModel;
        private readonly LanguageViewModel languageViewModel;
        private readonly LanguageStore languageStore;

        public ChangeLangCommand(SettingsViewModel settingsViewModel, LanguageViewModel languageViewModel, LanguageStore languageStore)
        {
            this.settingsViewModel = settingsViewModel;
            this.languageViewModel = languageViewModel;
            this.languageStore = languageStore;
        }

        public override void Execute(object? parameter)
        {
            languageStore.CurrentLanguage = languageViewModel.CultureInfo;
        }
    }
}
