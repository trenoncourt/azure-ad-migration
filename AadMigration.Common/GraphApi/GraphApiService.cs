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
        private readonly IOptions<TenantSettings> _tenantSettings;

        public GraphApiService(ILogger<TokenService> logger, IOptions<TenantSettings> tenantSettings)
        {
            _logger = logger;
            _tenantSettings = tenantSettings;
        }

        public async Task<IEnumerable<User>> GetUsersAsync(string token)
        {
            string baseUrl = $"{_tenantSettings.Value.Resource}/{_tenantSettings.Value.Tenant}";
            var azureAdGraphApi = RestService.For<IGraphApi>(baseUrl,
                new RefitSettings
                {
                    JsonSerializerSettings = new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    }
                });
            var response = await azureAdGraphApi.GetUsersAsync($"Bearer {token}");
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<IEnumerable<User>>(await response.Content.ReadAsStringAsync());
            }
            return null;
        }

        public Task AddUserAsync(User user, string token)
        {
            user.PasswordProfile = new Passwordprofile
            {
                ForceChangePasswordNextLogin = true,
                Password = _tenantSettings.Value.DefaultPassword
            };

            string baseUrl = $"{_tenantSettings.Value.Resource}/{_tenantSettings.Value.Tenant}";
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