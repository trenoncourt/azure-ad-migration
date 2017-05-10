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
        private readonly TenantSettings _tenantSettings;

        public TokenService(ILogger<TokenService> logger, TenantSettings tenantSettings)
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
                    Resource = _tenantSettings.Resource,
                    ClientId = _tenantSettings.ClientId,
                    ClientSecret = _tenantSettings.ClientSecret,
                    GrantType = "client_credentials"
                };

                string baseUrl = $"{_tenantSettings.Instance}{_tenantSettings.Tenant}";
                var azureAdGraphApi = RestService.For<ILoginApi>(baseUrl);
                return azureAdGraphApi.GetToken(tokenRequest);
            }
        }
    }
}