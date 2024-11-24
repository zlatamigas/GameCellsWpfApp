using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QueenGame.DB.DbContexts;
using QueenGame.Game.GameGenerator;
using QueenGame.Services.GameProvider;
using QueenGame.WPF.HostBuilders;
using QueenGame.WPF.Services;
using QueenGame.WPF.Stores;
using QueenGame.WPF.ViewModels;
using QueenGame.WPF.Views;
using System.Windows;

namespace QueenGame.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IHost? host;
        private LanguageStore languageStore;
        private IConfiguration configuration;

        protected override void OnStartup(StartupEventArgs e)
        {
            host = new AppHost();

            NavigationService<MainPageViewModel> navigationService
                = host.Services.GetRequiredService<NavigationService<MainPageViewModel>>();
            navigationService.Navigate();

            languageStore = host.Services.GetRequiredService<LanguageStore>();
            languageStore.LanguageChanged += OnAppLanguageChanged;

            configuration = host.Services.GetRequiredService<IConfiguration>();

            MainWindow = host.Services.GetRequiredService<MainWindowView>();
            MainWindow.Show();

            base.OnStartup(e);
        }

        private void OnAppLanguageChanged(object? sender, EventArgs e)
        {
            if (host != null)
            {
                configuration["DefaultLanguage"] = languageStore.CurrentLanguage.Name;
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (host != null)
            {
                configuration["DefaultLanguage"] = languageStore.CurrentLanguage.Name;
            }

            base.OnExit(e);
        }
    }

}
