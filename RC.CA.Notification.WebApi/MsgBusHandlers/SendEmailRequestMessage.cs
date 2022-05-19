using NT.CA.Notification.WebApi.Models;
using RC.CA.Infrastructure.MessageBus;

namespace NT.CA.Notification.WebApi.MsgBusHandlers
{
    public class SendEmailRequestMessage : IntegrationMessage
    {
        public EmailMessage EmailMessage { get; set; } = new EmailMessage();
    }
}
