using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Refit;

namespace AadMigration.Common.GraphApi
{
    public interface IGraphApi
    {
        [Get("/users?api-version={apiVersion}")]
        Task<HttpResponseMessage> GetUsersAsync([Header("Authorization")] string authorization, string apiVersion = "1.6");

        [Post("/users?api-version={apiVersion}")]
        Task PostUserAsync([Body] User user, [Header("Authorization")] string authorization, string apiVersion = "1.6");
    }
}