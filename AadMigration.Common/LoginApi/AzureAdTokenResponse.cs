using Newtonsoft.Json;

namespace AadMigration.Common.LoginApi
{
    public class AzureAdTokenResponse
    {
        [JsonProperty(PropertyName = "access_token")]
        public string Token { get; set; }
    }
}