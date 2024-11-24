using QueenGame.WPF.Commands;
using QueenGame.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QueenGame.WPF.ViewModels.ObjectViewModels
{
    public class LanguageViewModel : BaseViewModel
    {
        public ICommand ChangeLangCommand { get; }

        public CultureInfo CultureInfo { get; }
        private readonly SettingsViewModel settingsViewModel;
        private readonly LanguageStore languageStore;

        public string LanguageName => CultureInfo.DisplayName;

        public bool IsSelected => languageStore.CurrentLanguage.Equals(CultureInfo);

        public LanguageViewModel(CultureInfo cultureInfo, SettingsViewModel settingsViewModel, LanguageStore languageStore)
        {
            CultureInfo = cultureInfo;
            this.settingsViewModel = settingsViewModel;
            this.languageStore = languageStore;

            ChangeLangCommand = new ChangeLangCommand(settingsViewModel, this, languageStore);

            languageStore.LanguageChanged += OnLanguageChange;
        }

        private void OnLanguageChange(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(LanguageName));
            OnPropertyChanged(nameof(IsSelected));
        }
    }
}
