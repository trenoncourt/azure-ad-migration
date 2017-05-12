using System.Net.Http;
using System.Threading.Tasks;
using Refit;

namespace AadMigration.Common.GraphApi
{
    public interface IGraphApi
    {
        [Get("/users?$top=999&api-version={apiVersion}&$skiptoken={nextLink}")]
        Task<HttpResponseMessage> GetUsersAsync([Header("Authorization")] string authorization, string nextLink = "", string apiVersion = "1.6");

        [Post("/users?api-version={apiVersion}")]
        Task<HttpResponseMessage> PostUserAsync([Body] User user, [Header("Authorization")] string authorization, string apiVersion = "1.6");
    }
}