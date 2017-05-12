using Newtonsoft.Json;

namespace AadMigration.Common.LoginApi
{
    public class AuthError
    {
        public string Error { get; set; }

        [JsonProperty(PropertyName = "error_description")]
        public string Description { get; set; }
    }
}