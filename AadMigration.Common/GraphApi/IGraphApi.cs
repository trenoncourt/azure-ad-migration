using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Refit;

namespace AadMigration.Common.GraphApi
{
    public interface IGraphApi
    {
        [Get("/users/{userId}?api-version={apiVersion}")]
        Task<HttpResponseMessage> FindUserAsync(string userId, [Header("Authorization")] string authorization, string apiVersion = "1.6");

        [Get("/users?api-version={apiVersion}")]
        Task<IEnumerable<User>> GetUsersAsync([Header("Authorization")] string authorization, string apiVersion = "1.6");

        [Post("/users?api-version={apiVersion}")]
        Task PostUserAsync([Body] User user, [Header("Authorization")] string authorization, string apiVersion = "1.6");
    }
}