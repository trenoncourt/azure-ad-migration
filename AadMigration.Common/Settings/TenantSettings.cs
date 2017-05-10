namespace AadMigration.Common.Settings
{
    public class TenantSettings
    {
        public string Instance { get; set; }
        public string Tenant { get; set; }
        public string Resource { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string ApiVersion { get; set; }
        public string DefaultPassword { get; set; }
    }
}