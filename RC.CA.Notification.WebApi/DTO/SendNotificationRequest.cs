namespace NT.CA.Notification.WebApi.DTO
{
    /// <summary>
    /// Allow for expansion to other notification formats like sms...
    /// </summary>
    public class SendNotificationRequest
    {
        public List<EmailMessageRequest>? Emails { get; set; } = new List<EmailMessageRequest>();
        //public List<SmsMessageRequest>? SMS { get; set; } = new List<SmsMessageRequest>();
    }
}
