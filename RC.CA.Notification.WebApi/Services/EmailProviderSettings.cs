namespace NT.CA.Notification.WebApi.Services
{
    public class EmailProviderSettings
    {
        public string ApiKey { get; set; }
        public string SMTPServer { get; set; }
        public string SMTPServerPort { get; set; }
        public bool TLSEnabled { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
    }
}
