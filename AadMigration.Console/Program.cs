using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AadMigration.Console
{
    class Program
    {
        static void Main()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();
            IServiceProvider provider = Bootstrap.ConfigureServices(new ServiceCollection(), configuration);
            
            provider
                .GetService<ILoggerFactory>()
                .AddConsole(configuration.GetSection("Logging"));
        }
    }
}