using System.Net.Http;
using System.Threading.Tasks;
using Refit;

namespace AadMigration.Common.LoginApi
{
    public interface ILoginApi
    {
        [Get("/oauth2/token")]
        Task<HttpResponseMessage> GetToken([Body(BodySerializationMethod.UrlEncoded)] AzureAdTokenRequest tokenRequest);
    }
}