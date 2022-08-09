namespace RC.CA.Application.Settings
{
    public class BlobStorageSettings
    {
        public string ConnectionString { get; set; } = "";
        public string ContainerName { get; set; } = "";
        public string ContainerNameFiles { get; set; } = "";
        public string CdnEndpoint { get; set; } = "";
    }
}
