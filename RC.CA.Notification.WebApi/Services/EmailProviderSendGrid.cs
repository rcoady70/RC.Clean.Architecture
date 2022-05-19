using Microsoft.Extensions.Options;
using NT.CA.Notification.WebApi.MsgBusHandlers;
using RC.CS.Email.Data.ValueTypes;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace NT.CA.Notification.WebApi.Services
{
    public class EmailProviderSendGrid : IEmailProvider
    {
        private readonly IOptions<EmailProviderSettings> _emailProviderSettings;

        public EmailProviderSendGrid(IOptions<EmailProviderSettings> emailProviderSettings)
        {
            _emailProviderSettings = emailProviderSettings;
        }

        public async Task<bool> SendSingleEmailAsync(SendEmailRequestMessage sendEMailMessage)
        {
            bool sendOK = false;
            var emailMessage = sendEMailMessage.EmailMessage;

            var apiKey = _emailProviderSettings.Value.ApiKey;
            var client = new SendGridClient(apiKey);

            var from = new EmailAddress(emailMessage.From.Address, emailMessage.From.DisplayName);
            var subject = emailMessage.Subject;

            List<EmailAddress> tos = new List<EmailAddress>();
            foreach (var to in emailMessage.To)
                tos.Add(new EmailAddress(to.Address, to.DisplayName));

            List<EmailAddress> replyTos = new List<EmailAddress>();
            foreach (var replyTo in emailMessage.To)
                replyTos.Add(new EmailAddress(replyTo.Address, replyTo.DisplayName));

            var plainTextContent = "";
            var htmlContent = "";
            foreach (var ietm in emailMessage.Content)
            {
                switch (ietm.ContentType)
                {
                    case EmailContentType.Textplain: plainTextContent = ietm.EmailContent; break;
                    case EmailContentType.Texthtml: htmlContent = ietm.EmailContent; break;
                    default: plainTextContent = ietm.EmailContent; break;
                }
            }

            var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, tos, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

            if (response != null)
                sendOK = response.IsSuccessStatusCode;

            return sendOK;
        }
    }
}
