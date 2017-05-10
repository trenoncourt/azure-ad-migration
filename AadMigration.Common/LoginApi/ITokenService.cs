using System.Threading.Tasks;

namespace AadMigration.Common.LoginApi
{
    public interface ITokenService
    {
        Task<AzureAdTokenResponse> GetAsync();
    }
}