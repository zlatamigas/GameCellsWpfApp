using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using QueenGame.WPF.Utils.JSON;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueenGame.WPF.HostBuilders
{
    public static class AppConfigurationExtensions
    {
        private static string APPSETTINGS_FILENAME = "appsettings.json";

        public static IHostBuilder AddAppConfiguration(this IHostBuilder hostBuilder)
        {
            IConfiguration appSettings = GetFileConfiguration();
            
            return hostBuilder
                .ConfigureAppConfiguration((confugureDelagates) =>
                {
                    confugureDelagates.AddConfiguration(appSettings);
                })
                .ConfigureServices(services => 
                {
                    services.AddSingleton(appSettings);
                });
            }

        private static IConfiguration GetFileConfiguration()
        {
            if (File.Exists(APPSETTINGS_FILENAME))
            {
                try
                {
                    JsonConvert.DeserializeObject(File.ReadAllText(APPSETTINGS_FILENAME));
                }
                catch (Exception)
                {
                    using (var f = File.CreateText(APPSETTINGS_FILENAME))
                    {
                        f.WriteLine("{}");
                    }
                }
            }
            else
            {
                using (var f = File.CreateText(APPSETTINGS_FILENAME))
                {
                    f.WriteLine("{}");
                }
            }

            IConfiguration appSettings = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .Add<WritableJsonConfigurationSource>(s => 
                {
                    s.FileProvider = null;
                    s.Path = APPSETTINGS_FILENAME;
                })
                .Build();

            return appSettings;
        }
    }
}
