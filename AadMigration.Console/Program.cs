using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AadMigration.Common.GraphApi;
using AadMigration.Common.LoginApi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AadMigration.Console
{
    class Program
    {
        static void Main()
        {
            MainAsync().GetAwaiter().GetResult();
        }

        public static async Task MainAsync()
        {

            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();
            IServiceProvider provider = Bootstrap.ConfigureServices(new ServiceCollection(), configuration);

            provider
                .GetService<ILoggerFactory>()
                .AddConsole(configuration.GetSection("Logging"));

            var tokenService = provider.GetService<ITokenService>();
            AzureAdTokenResponse tokenResponse = await tokenService.GetAsync();

            var graphApiService = provider.GetService<IGraphApiService>();
            IEnumerable<User> users = await graphApiService.GetUsersAsync(tokenResponse.Token);
        }
    }
}