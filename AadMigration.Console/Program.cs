using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AadMigration.Common.GraphApi;
using AadMigration.Common.LoginApi;
using AadMigration.Common.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AadMigration.Console
{
    class Program
    {
        private static IServiceProvider _provider;
        private static ILogger _logger;

        static void Main()
        {
            try
            {
                MainAsync().GetAwaiter().GetResult();
            }
            catch (Exception)
            {
                // ignored
            }
            System.Console.WriteLine("Press any key to continue...");
            System.Console.Read();
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

            _logger = _provider.GetService<ILogger<Program>>();

            // Get AAD API token.
            var tokenService = _provider.GetService<ITokenService>();
            var settingsFrom = _provider.GetService<TenantSettingsFrom>();
            AzureAdTokenResponse tokenResponseFrom = await tokenService.GetAsync(settingsFrom);

            // Get All Users.
            IEnumerable<User> users = await GetUsersAsync(tokenResponseFrom.Token);

            var settingsTo = _provider.GetService<TenantSettingsTo>();
            AzureAdTokenResponse tokenResponseTo = await tokenService.GetAsync(settingsTo);

            await ImportUsersAsync(users, tokenResponseTo.Token);
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

        private static async Task ImportUsersAsync(IEnumerable<User> users, string token)
        {
            var graphApiService = _provider.GetService<IGraphApiService>();

            foreach (var user in users)
            {
                _logger.LogInformation($"Import user: {user.DisplayName} / {user.UserPrincipalName}");
                await graphApiService.AddUserAsync(user, token);
            }
        }
    }
}