using System.Threading.Tasks;
using Refit;

namespace AadMigration.Common.LoginApi
{
    public interface ILogin
    {
        [Get("/oauth2/token")]
        Task<AzureAdTokenResponse> GetToken([Body(BodySerializationMethod.UrlEncoded)] AzureAdTokenRequest tokenRequest);
    }
}