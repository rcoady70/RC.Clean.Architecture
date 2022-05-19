using NT.CA.Notification.WebApi.Services;
using RC.CA.Infrastructure.MessageBus.Interfaces;
using RC.CA.SharedKernel.Exceptions;

namespace NT.CA.Notification.WebApi.MsgBusHandlers
{
    public class SendEmailRequestHandler : IIntegrationEventHandler<SendEmailRequestMessage>
    {
        private readonly IEmailProvider _emailProvider;

        public SendEmailRequestHandler(IEmailProvider emailProvider)
        {
            _emailProvider = emailProvider;
        }
        /// <summary>
        /// Process message from queue
        /// </summary>
        /// <param name="eventMessage"></param>
        /// <returns></returns>
        public async Task Handle(SendEmailRequestMessage eventMessage)
        {
            var sent = await _emailProvider.SendSingleEmailAsync(eventMessage);
            if (!sent)
                throw new EmailException(eventMessage.EmailMessage.From.ToString(), 
                                         eventMessage.EmailMessage.To[0].ToString(),
                                         eventMessage.EmailMessage.Subject,
                                         nameof(NT.CA.Notification.WebApi.Services.EmailProviderSendGrid),null);
        }
    }
}
