using QueenGame.WPF.Commands;
using QueenGame.WPF.Services;
using QueenGame.WPF.Stores;
using QueenGame.WPF.ViewModels.ObjectViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace QueenGame.WPF.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private ObservableCollection<LanguageViewModel> _Languages;
        public IEnumerable<LanguageViewModel> Languages => _Languages;

        public string SelectedLanguage => languageStore.CurrentLanguage.ThreeLetterISOLanguageName.ToUpperInvariant();


        public ICommand ToMainPageViewModelCommand { get; }
        public ICommand ShowLangsCommand { get; }

        private bool _LangsShown;
        private readonly LanguageStore languageStore;

        public bool LangsShown
        {
            get { return _LangsShown; }
            set
            {
                _LangsShown = value;
                OnPropertyChanged(nameof(LangsShown));
            }
        }

        public static SettingsViewModel LoadViewModel(NavigationService<MainPageViewModel> navigationService, LanguageStore languageStore)
        {
            var viewModel = new SettingsViewModel(navigationService, languageStore);

            return viewModel;
        }


        public SettingsViewModel(NavigationService<MainPageViewModel> navigationService, LanguageStore languageStore) 
        {
            this.languageStore = languageStore;
            _Languages = new ObservableCollection<LanguageViewModel>();
            foreach (var language in languageStore.Languages) 
            { 
                _Languages.Add(new LanguageViewModel(language, this, languageStore));
            }

            ToMainPageViewModelCommand = new NavigationCommand<MainPageViewModel>(navigationService);
            ShowLangsCommand = new ShowLangsCommand(this);

            LangsShown = false;

            languageStore.LanguageChanged += OnLanguageChanged;
        }

        private void FillLanguages() 
        {
            _Languages.Clear();
            foreach (var language in languageStore.Languages)
            {
                _Languages.Add(new LanguageViewModel(language, this, languageStore));
            }
        }

        private void OnLanguageChanged(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof (SelectedLanguage));
            //FillLanguages();
            OnPropertyChanged(nameof (Languages));
        }
    }
}
