using Newtonsoft.Json;

namespace AadMigration.Common.GraphApi
{
    public class OdataWrapper<T>
    {
        public T Value { get; set; }

        [JsonProperty(PropertyName = "odata.nextLink")]
        public string NextLink { get; set; }
    }
}