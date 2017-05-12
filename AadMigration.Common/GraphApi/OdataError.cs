using Newtonsoft.Json;

namespace AadMigration.Common.GraphApi
{
    public class OdataError
    {
        [JsonProperty(PropertyName = "odata.error")]
        public Error Error { get; set; }
    }

    public class Error
    {
        public string Code { get; set; }

        public ErrorMessage Message { get; set; }
    }

    public class ErrorMessage
    {
        public string Value { get; set; }
    }
}