using System.Collections.Generic;
using System.Threading.Tasks;
using AadMigration.Common.LoginApi;
using AadMigration.Common.Settings;
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
        private readonly TenantSettings _tenantSettings;

        public GraphApiService(ILogger<TokenService> logger, TenantSettings tenantSettings)
        {
            _logger = logger;
            _tenantSettings = tenantSettings;
        }

        public async Task<OdataWrapper<IEnumerable<User>>> GetUsersAsync(string token, string nextLink = "")
        {
            string baseUrl = $"{_tenantSettings.Resource}/{_tenantSettings.Tenant}";
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
            user.PasswordProfile = new Passwordprofile
            {
                ForceChangePasswordNextLogin = true,
                Password = _tenantSettings.DefaultPassword
            };

            string baseUrl = $"{_tenantSettings.Resource}/{_tenantSettings.Tenant}";
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