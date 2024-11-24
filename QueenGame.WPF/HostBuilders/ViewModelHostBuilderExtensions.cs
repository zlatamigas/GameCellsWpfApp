using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QueenGame.Game.GameGenerator;
using QueenGame.Services.GameInfoProvider;
using QueenGame.Services.GameProvider;
using QueenGame.Services.GeneratedGameResultProvider;
using QueenGame.WPF.Services;
using QueenGame.WPF.Stores;
using QueenGame.WPF.ViewModels;
using QueenGame.WPF.Views;

namespace QueenGame.WPF.HostBuilders
{
    public static class ViewModelHostBuilderExtensions
    {
        public static IHostBuilder AddViewModels(this IHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureServices(services => {
                // Window
                services.AddSingleton<MainWindowViewModel>();
                services.AddSingleton((s) =>
                   new MainWindowView()
                   {
                       DataContext = new MainWindowViewModel(
                           s.GetRequiredService<NavigationStore>())
                   }
                );
                // ViewModel (Transient - чтобы View и ViewModel уничтожались в жизненном цикле)
                services.AddTransient<MainPageViewModel>((s) => GetMainPageViewModel(s));
                services.AddTransient<LevelsListingViewModel>((s) => GetLevelsListingViewModel(s));
                services.AddTransient<GenerateLevelOptionsViewModel>((s) => GetGenerateLevelOptionsViewModel(s));
                services.AddTransient<LevelViewModel>((s) => GetLevelViewModel(s));
                services.AddTransient<SettingsViewModel>((s) => GetSettingsViewModel(s));
                // Func
                services.AddSingleton<Func<MainPageViewModel>>((s) => s.GetRequiredService<MainPageViewModel>);
                services.AddSingleton<Func<LevelsListingViewModel>>((s) => s.GetRequiredService<LevelsListingViewModel>);
                services.AddSingleton<Func<GenerateLevelOptionsViewModel>>((s) => s.GetRequiredService<GenerateLevelOptionsViewModel>);
                services.AddSingleton<Func<LevelViewModel>>((s) => s.GetRequiredService<LevelViewModel>);
                services.AddSingleton<Func<SettingsViewModel>>((s) => s.GetRequiredService<SettingsViewModel>);
                // NavigationService
                services.AddSingleton<NavigationService<MainPageViewModel>>();
                services.AddSingleton<NavigationService<LevelsListingViewModel>>();
                services.AddSingleton<NavigationService<GenerateLevelOptionsViewModel>>();
                services.AddSingleton<NavigationService<LevelViewModel>>();
                services.AddSingleton<NavigationService<SettingsViewModel>>();
            });
        }


        private static MainPageViewModel GetMainPageViewModel(IServiceProvider serviceProvider)
        {
            return MainPageViewModel.LoadViewModel(
                serviceProvider.GetRequiredService<NavigationService<LevelsListingViewModel>>(),
                serviceProvider.GetRequiredService<NavigationService<GenerateLevelOptionsViewModel>>(),
                serviceProvider.GetRequiredService<NavigationService<SettingsViewModel>>()
                );
        }

        private static LevelsListingViewModel GetLevelsListingViewModel(IServiceProvider serviceProvider)
        {
            return LevelsListingViewModel.LoadViewModel(
                serviceProvider.GetRequiredService<NavigationStore>(),
                serviceProvider.GetRequiredService<IGameInfoProvider>(),
                serviceProvider.GetRequiredService<NavigationService<MainPageViewModel>>(),
                serviceProvider.GetRequiredService<NavigationService<LevelViewModel>>(),
                serviceProvider.GetRequiredService<LevelStore>(),
                serviceProvider.GetRequiredService<LevelsInfoStore>()
                );
        }

        private static GenerateLevelOptionsViewModel GetGenerateLevelOptionsViewModel(IServiceProvider serviceProvider)
        {
            return GenerateLevelOptionsViewModel.LoadViewModel(
                serviceProvider.GetRequiredService<NavigationStore>(),
                serviceProvider.GetRequiredService<NavigationService<MainPageViewModel>>(),
                serviceProvider.GetRequiredService<NavigationService<LevelViewModel>>(),
                serviceProvider.GetRequiredService<IGameGenerator>(),
                serviceProvider.GetRequiredService<LevelStore>(),
                serviceProvider.GetRequiredService<IGeneratedGameResultProvider>()
                );
        }

        private static LevelViewModel GetLevelViewModel(IServiceProvider serviceProvider)
        {
            return LevelViewModel.LoadViewModel(
                serviceProvider.GetRequiredService<LevelStore>(),
                serviceProvider.GetRequiredService<LevelsInfoStore>(),
                serviceProvider.GetRequiredService<NavigationService<GenerateLevelOptionsViewModel>>(),
                serviceProvider.GetRequiredService<NavigationService<LevelsListingViewModel>>(),
                serviceProvider.GetRequiredService<NavigationService<LevelViewModel>>()
                );
        }

        private static SettingsViewModel GetSettingsViewModel(IServiceProvider serviceProvider)
        {
            return SettingsViewModel.LoadViewModel(
                serviceProvider.GetRequiredService<NavigationService<MainPageViewModel>>(),
                serviceProvider.GetRequiredService<LanguageStore>()
                );
        }
    }
}
