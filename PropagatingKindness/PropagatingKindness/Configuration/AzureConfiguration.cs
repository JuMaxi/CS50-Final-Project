namespace PropagatingKindness.Configuration;

public class AzureConfiguration
{
    internal const string SectionKey = "Azure";
    public string tenantId { get; set; }
    public string clientId { get; set; }
    public string clientSecret { get; set; }
    public string StorageAccountURL { get; set; }
}
