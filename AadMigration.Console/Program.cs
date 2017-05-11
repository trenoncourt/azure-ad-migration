using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private static IServiceProvider _provider;

        static void Main()
        {
            MainAsync().GetAwaiter().GetResult();
        }

        public static async Task MainAsync()
        {
            // Configure DI.
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();
            _provider = Bootstrap.ConfigureServices(new ServiceCollection(), configuration);

            _provider
                .GetService<ILoggerFactory>()
                .AddConsole(configuration.GetSection("Logging"));

            // Get AAD API token.
            var tokenService = _provider.GetService<ITokenService>();
            AzureAdTokenResponse tokenResponse = await tokenService.GetAsync();

            // Get All Users.
            IEnumerable<User> users = await GetUsersAsync(tokenResponse.Token);

        }

        private static async Task<IEnumerable<User>> GetUsersAsync(string token)
        {
            var graphApiService = _provider.GetService<IGraphApiService>();
            OdataWrapper<IEnumerable<User>> response = await graphApiService.GetUsersAsync(token);
            string nextLink = response.NextLink;

            List<User> users = response.Value.ToList();
            while (!string.IsNullOrEmpty(nextLink))
            {
                response = await graphApiService.GetUsersAsync(token, nextLink);
                nextLink = response.NextLink;
                users.AddRange(response.Value);
            }
            return users;
        }
    }
}