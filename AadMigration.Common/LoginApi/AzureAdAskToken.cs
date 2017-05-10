using Refit;

namespace AadMigration.Common.LoginApi
{
    public class AzureAdTokenRequest
    {
        [AliasAs("grant_type")]
        public string GrantType { get; set; }

        [AliasAs("client_id")]
        public string ClientId { get; set; }

        [AliasAs("client_secret")]
        public string ClientSecret { get; set; }

        [AliasAs("resource")]
        public string Resource { get; set; }
    }
}