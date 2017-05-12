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
            var container = new Container().WithDependencyInjectionAdapter(services);

            container.Register<IGraphApiService, GraphApiService>(reuse: Reuse.Singleton);
            container.Register<ITokenService, TokenService>(reuse:Reuse.Singleton);

            container.RegisterDelegate(resolver => configuration.GetSection("TenantSettingsFrom").Get<TenantSettingsFrom>(), reuse:Reuse.Singleton);
            container.RegisterDelegate(resolver => configuration.GetSection("TenantSettingsTo").Get<TenantSettingsTo>(), reuse:Reuse.Singleton);
           
            return container.Resolve<IServiceProvider>();
        }
    }
}