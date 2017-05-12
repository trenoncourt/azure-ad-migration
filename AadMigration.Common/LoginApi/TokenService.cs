using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AadMigration.Common.GraphApi;
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

        public async Task<AzureAdTokenResponse> GetAsync(TenantSettings tennantSetting)
        {
            using (_logger.BeginScope("Get AAD token."))
            {
                _logger.LogDebug($"Tenant settings used : {JsonConvert.SerializeObject(tennantSetting)}");
                var tokenRequest = new AzureAdTokenRequest
                {
                    Resource = tennantSetting.Resource,
                    ClientId = tennantSetting.ClientId,
                    ClientSecret = tennantSetting.ClientSecret,
                    GrantType = "client_credentialsa"
                };

                string baseUrl = $"{tennantSetting.Instance}{tennantSetting.Tenant}";
                var loginApi = RestService.For<ILoginApi>(baseUrl);

                HttpResponseMessage response = await loginApi.GetToken(tokenRequest);
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<AzureAdTokenResponse>(await response.Content.ReadAsStringAsync());
                }

                var error = JsonConvert.DeserializeObject<AuthError>(await response.Content.ReadAsStringAsync());
                _logger.LogError(
                    $"Unable to get token. Error: {error.Error}, Description: {error.Description}");
                response.EnsureSuccessStatusCode();
                return null;
            }
        }
    }
}