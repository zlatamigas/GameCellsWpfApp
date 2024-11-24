using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QueenGame.DB.DbContexts;
using QueenGame.Game.GameGenerator;
using QueenGame.Services.GameInfoProvider;
using QueenGame.Services.GameProvider;
using QueenGame.Services.GameResultProvider;
using QueenGame.Services.GeneratedGameResultProvider;
using QueenGame.WPF.Stores;

namespace QueenGame.WPF.HostBuilders
{
    public class AppHost : IHost
    {
        private readonly IHost host;

        public AppHost()
        {
            host = Host.CreateDefaultBuilder()
                .AddViewModels()
                .AddAppConfiguration()
                .ConfigureServices((hostContext, services) =>
                {
                    // Localisation
                    List<string> langStrs = hostContext.Configuration.GetSection("Languages").Get<List<string>>() ?? new List<string>(0);
                    string? prefferedLang = hostContext.Configuration.GetValue<string?>("DefaultLanguage");
                    services.AddSingleton(new LanguageStore(langStrs, prefferedLang));
                    // DB
                    string connectionString = hostContext.Configuration.GetConnectionString("Default") ?? String.Empty;
                    services.AddSingleton(new AppDbContextFactory(connectionString));
                    // Services
                    services.AddSingleton<IGameProvider>((s) => 
                        new DatabaseGameProvider(s.GetRequiredService<AppDbContextFactory>()));
                    services.AddSingleton<IGameInfoProvider>((s) => 
                        new DatabaseGameInfoProvider(s.GetRequiredService<AppDbContextFactory>()));
                    services.AddSingleton<IGameResultProvider>((s) => 
                        new DatabaseGameResultProvider(s.GetRequiredService<AppDbContextFactory>()));
                    services.AddSingleton<IGeneratedGameResultProvider>((s) => 
                        new DatabaseGeneratedGameResultProvider(s.GetRequiredService<AppDbContextFactory>()));
                    services.AddSingleton<IGameGenerator, AxesAndNearestGameGenerator>();
                    // Stores
                    services.AddSingleton<NavigationStore>();
                    services.AddSingleton<LevelStore>();
                    services.AddSingleton<LevelsInfoStore>();
                })
                .Build();
        }

        public IServiceProvider Services => host.Services;

        public void Dispose()
        {
            host.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken = default)
        {
            return host.StartAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken = default)
        {
            return host.StopAsync(cancellationToken);
        }
    }
}
