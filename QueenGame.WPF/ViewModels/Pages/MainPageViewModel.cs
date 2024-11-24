using QueenGame.WPF.Commands;
using QueenGame.WPF.Services;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace QueenGame.WPF.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        public ICommand ToLevelsListingViewModelCommand { get;  }
        public ICommand ToGenerateLevelOptionsViewModelCommand { get;  }
        public ICommand ToSettingsViewModelCommand { get;  }

        public string? Version { get; }

        public static MainPageViewModel LoadViewModel(
            NavigationService<LevelsListingViewModel> navigationServiceLevelsListing,
            NavigationService<GenerateLevelOptionsViewModel> navigationServiceGenerateLevelOptions,
            NavigationService<SettingsViewModel> navigationServiceSettings)
        {
            MainPageViewModel viewModel = new MainPageViewModel(
                navigationServiceLevelsListing, 
                navigationServiceGenerateLevelOptions,
                navigationServiceSettings);

            return viewModel;
        }

        private MainPageViewModel(
            NavigationService<LevelsListingViewModel> navigationServiceLevelsListing,
            NavigationService<GenerateLevelOptionsViewModel> navigationServiceGenerateLevelOptions,
            NavigationService<SettingsViewModel> navigationServiceSettings
            )
        {
            ToLevelsListingViewModelCommand = new NavigationCommand<LevelsListingViewModel>(navigationServiceLevelsListing);
            ToGenerateLevelOptionsViewModelCommand = new NavigationCommand<GenerateLevelOptionsViewModel>(navigationServiceGenerateLevelOptions);
            ToSettingsViewModelCommand = new NavigationCommand<SettingsViewModel>(navigationServiceSettings);

            Version = $"v {Assembly.GetExecutingAssembly().GetName()?.Version?.ToString()}";
        }
    }
}
