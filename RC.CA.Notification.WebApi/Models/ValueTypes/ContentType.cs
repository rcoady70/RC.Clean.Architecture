namespace RC.CS.Email.Data.ValueTypes
{
    public class Content
    {
        public Content(EmailContentType contentType, string emailContent)
        {
            ContentType = contentType;
            EmailContent = emailContent;
        }

        public EmailContentType ContentType { get; set; }
        public string EmailContent { get; set; }
    }
}
