using System;
using AadMigration.Common.GraphApi;
using AadMigration.Common.LoginApi;
using AadMigration.Common.Settings;
using DryIoc;
using DryIoc.Microsoft.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AadMigration.Console
{
    public static class Bootstrap
    {
        public static IServiceProvider ConfigureServices(IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddLogging();

            services.Configure<TenantSettings>(configuration.GetSection("TenantSettings"));

            var container = new Container().WithDependencyInjectionAdapter(services);

            container.Register<IGraphApiService, GraphApiService>();
            container.Register<ITokenService, TokenService>();

            return container.Resolve<IServiceProvider>();
        }
    }
}