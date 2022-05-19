using RC.CA.SharedKernel.ValueTypes;
using RC.CS.Email.Data.ValueTypes;

namespace NT.CA.Notification.WebApi.DTO
{
    public class EmailMessageRequest
    {
        /// <summary>
        /// From email
        /// </summary>
        public EmailAddress From { get; set; } = new EmailAddress("fromemail@email.com","John From");
        /// <summary>
        /// To email address
        /// </summary>
        public List<EmailAddress> To { get; set; } = new List<EmailAddress>() { new EmailAddress("toemail@email.com", "John To") };
        /// <summary>
        /// Gets or sets the subject of your email. This may be overridden by personalizations[x].subject.
        /// </summary>
        public string Subject { get; set; } = "Enter your subject here";
        /// <summary>
        /// Email content 
        /// </summary>
        public List<Content> Content { get; set; } = new List<Content>() { new Content(EmailContentType.Textplain, "This is a plain text email message"), new Content(EmailContentType.Texthtml, "<strong>This is a html message</strong>") };

        /// <summary>
        /// Reply to
        /// </summary>
        public List<EmailAddress>? ReplyTo { get; set; } = new List<EmailAddress>();
        
        /// <summary>
        /// Gets or sets an object containing key/value pairs of header names and the value to substitute for them.
        /// </summary>
        public Dictionary<string, string>? Headers { get; set; } = new Dictionary<string, string>();
    }
}
