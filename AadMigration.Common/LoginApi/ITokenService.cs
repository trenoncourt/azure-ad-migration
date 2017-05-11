using System.Threading.Tasks;
using AadMigration.Common.Settings;

namespace AadMigration.Common.LoginApi
{
    public interface ITokenService
    {
        Task<AzureAdTokenResponse> GetAsync(TenantSettings tennantSetting);
    }
}