using System.Threading.Tasks;
using AadMigration.Common.Settings;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Refit;

namespace AadMigration.Common.LoginApi
{
    public class TokenService : ITokenService
    {
        private readonly ILogger<TokenService> _logger;

        public TokenService(ILogger<TokenService> logger)
        {
            _logger = logger;
        }

        public Task<AzureAdTokenResponse> GetAsync(TenantSettings tennantSetting)
        {
            using (_logger.BeginScope("Get AAD token."))
            {
                _logger.LogDebug($"Tenant settings used : {JsonConvert.SerializeObject(tennantSetting)}");
                var tokenRequest = new AzureAdTokenRequest
                {
                    Resource = tennantSetting.Resource,
                    ClientId = tennantSetting.ClientId,
                    ClientSecret = tennantSetting.ClientSecret,
                    GrantType = "client_credentials"
                };

                string baseUrl = $"{tennantSetting.Instance}{tennantSetting.Tenant}";
                var azureAdGraphApi = RestService.For<ILoginApi>(baseUrl);
                return azureAdGraphApi.GetToken(tokenRequest);
            }
        }
    }
}