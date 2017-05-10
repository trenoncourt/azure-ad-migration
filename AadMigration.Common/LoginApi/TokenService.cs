using System.Threading.Tasks;
using AadMigration.Common.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Refit;

namespace AadMigration.Common.LoginApi
{
    public class TokenService : ITokenService
    {
        private readonly ILogger<TokenService> _logger;
        private readonly IOptions<TenantSettings> _tenantSettings;

        public TokenService(ILogger<TokenService> logger, IOptions<TenantSettings> tenantSettings)
        {
            _logger = logger;
            _tenantSettings = tenantSettings;
        }

        public Task<AzureAdTokenResponse> GetAsync()
        {
            using (_logger.BeginScope("Get AAD token."))
            {
                _logger.LogDebug($"Tenant settings used : {JsonConvert.SerializeObject(_tenantSettings)}");
                var tokenRequest = new AzureAdTokenRequest
                {
                    Resource = _tenantSettings.Value.Resource,
                    ClientId = _tenantSettings.Value.ClientId,
                    ClientSecret = _tenantSettings.Value.ClientSecret,
                    GrantType = "client_credentials"
                };

                string baseUrl = $"{_tenantSettings.Value.Instance}{_tenantSettings.Value.Tenant}";
                var azureAdGraphApi = RestService.For<ILoginApi>(baseUrl);
                return azureAdGraphApi.GetToken(tokenRequest);
            }
        }
    }
}