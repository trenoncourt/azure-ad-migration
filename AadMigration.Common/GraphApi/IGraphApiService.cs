using System.Collections.Generic;
using System.Threading.Tasks;

namespace AadMigration.Common.GraphApi
{
    public interface IGraphApiService
    {
        Task<OdataWrapper<IEnumerable<User>>> GetUsersAsync(string token, string nextLink = "");

        Task AddUserAsync(User user, string token);
    }
}