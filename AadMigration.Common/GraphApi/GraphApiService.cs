using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AadMigration.Common.LoginApi;
using AadMigration.Common.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Refit;

namespace AadMigration.Common.GraphApi
{
    public class GraphApiService : IGraphApiService
    {
        private readonly ILogger<TokenService> _logger;
        private readonly IServiceProvider _provider;

        public GraphApiService(ILogger<TokenService> logger, IServiceProvider provider)
        {
            _logger = logger;
            _provider = provider;
        }

        public async Task<OdataWrapper<IEnumerable<User>>> GetUsersAsync(string token, string nextLink = "")
        {
            var tenantSettings = _provider.GetService<TenantSettingsFrom>();
            string baseUrl = $"{tenantSettings.Resource}/{tenantSettings.Tenant}";
            var azureAdGraphApi = RestService.For<IGraphApi>(baseUrl,
                new RefitSettings
                {
                    JsonSerializerSettings = new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    }
                });
            var response = await azureAdGraphApi.GetUsersAsync($"Bearer {token}", nextLink);
            if (response.IsSuccessStatusCode)
            {
                var wrapper = JsonConvert.DeserializeObject<OdataWrapper<IEnumerable<User>>>(await response.Content.ReadAsStringAsync());
                wrapper.NextLink = wrapper.NextLink
                    ?.Replace("directoryObjects/$/Microsoft.DirectoryServices.User?$skiptoken=", "");
                return wrapper;
            }
            return null;
        }

        public Task AddUserAsync(User user, string token)
        {
            var tenantSettingsFrom = _provider.GetService<TenantSettingsFrom>();
            var tenantSettingsTo = _provider.GetService<TenantSettingsTo>();
            user.PasswordProfile = new Passwordprofile
            {
                ForceChangePasswordNextLogin = true,
                Password = tenantSettingsTo.DefaultPassword
            };

            user.UserPrincipalName = user.UserPrincipalName.Replace(tenantSettingsFrom.Tenant, tenantSettingsTo.Tenant);

            string baseUrl = $"{tenantSettingsTo.Resource}/{tenantSettingsTo.Tenant}";
            var azureAdGraphApi = RestService.For<IGraphApi>(baseUrl,
                new RefitSettings
                {
                    JsonSerializerSettings = new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    }
                });
            return azureAdGraphApi.PostUserAsync(user, $"Bearer {token}");
        }
    }
}