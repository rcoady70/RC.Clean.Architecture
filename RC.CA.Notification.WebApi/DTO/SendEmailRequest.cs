using NT.CA.Notification.WebApi.Models;

namespace NT.CA.Notification.WebApi.DTO
{
    public class SendEmailMessageRequest
    {
        public EmailMessageRequest EmailMessage { get; set; } = new EmailMessageRequest();
    }
}
