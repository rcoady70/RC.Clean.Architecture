using RC.CA.SharedKernel.GuardClauses;
using RC.CA.SharedKernel.ValueTypes;
using RC.CS.Email.Data.ValueTypes;

namespace NT.CA.Notification.WebApi.Models;

public class EmailMessage
{
    /// <summary>
    /// From email
    /// </summary>
    public EmailAddress From { get; set; }
    /// <summary>
    /// To email address
    /// </summary>
    public List<EmailAddress> To { get; set; }
    /// <summary>
    /// Copy to
    /// </summary>
    public List<EmailAddress> CC { get; set; } = new List<EmailAddress>();
    /// <summary>
    /// Blind carbon copy
    /// </summary>
    public List<EmailAddress> BCC { get; set; } = new List<EmailAddress>();
    /// <summary>
    /// Reply to
    /// </summary>
    public List<EmailAddress> ReplyTo { get; set; } = new List<EmailAddress>();

    /// <summary>
    /// Gets or sets the subject of your email. This may be overridden by personalizations[x].subject.
    /// </summary>
    public string Subject { get; set; }

    /// <summary>
    /// Email content 
    /// </summary>
    public List<Content> Content { get; set; }

    /// <summary>
    /// Gets or sets an object containing key/value pairs of header names and the value to substitute for them.
    /// </summary>
    public Dictionary<string, string> Headers { get; set; }

    /// <summary>
    /// Delay email send by x minutes
    /// </summary>
    public int? DelayByMinutes { get; set; } = 0;

    public void SetDelayByMinutes(int delayBy) 
    {
        DelayByMinutes = delayBy;
    }
    /// <summary>
    /// Add content
    /// </summary>
    /// <param name="content"></param>
    public void AddHeader(Content content)
    {
        Guard.Against.Null(content, nameof(content));
        Content.Add(content);
    }

    /// <summary>
    /// Add a header to the email.
    /// </summary>
    /// <param name="headerKey">Header key (e.g. X-Header).</param>
    /// <param name="headerValue">Header value.</param>
    public void AddHeader(string headerKey, string headerValue)
    {
        Guard.Against.Null(headerKey, nameof(headerKey));
        Guard.Against.Null(headerValue, nameof(headerValue));
        Headers.Add(headerKey, headerValue);
    }

    /// <summary>
    /// Add headers to the email.
    /// </summary>
    /// <param name="headers">A list of Headers.</param>
    public void AddHeaders(Dictionary<string, string> headers, int personalizationIndex = 0)
    {
        Guard.Against.Null(headers, nameof(headers));
        Headers.Union(headers).ToDictionary(pair => pair.Key, pair => pair.Value);
    }

    public void SetFrom(string email,string? displayName)
    {
        Guard.Against.Null(email,nameof(email));

        SetFrom(new EmailAddress(email, displayName ?? ""));
    }

    /// <summary>
    /// Set the from email.
    /// </summary>
    /// <param name="email">An email object containing the email address and name of the sender. Unicode encoding is not supported for the from field.</param>
    public void SetFrom(EmailAddress email)
    {
        Guard.Against.Null(email, nameof(email));
        From = email;
    }
    /// <summary>
    /// Add reply cc
    /// </summary>
    /// <param name="email"></param>
    public void AddCC(EmailAddress email)
    {
        Guard.Against.Null(email, nameof(email));
        AddEmailToGroup(email, CC);
    }
    /// <summary>
    /// Add reply to
    /// </summary>
    /// <param name="email"></param>
    public void AddReplyTo(EmailAddress email)
    {
        Guard.Against.Null(email, nameof(email));
        AddEmailToGroup(email, ReplyTo);
    }
    /// <summary>
    /// Add bcc
    /// </summary>
    /// <param name="email"></param>
    public void AddBcc(EmailAddress email)
    {
        Guard.Against.Null(email, nameof(email));
        AddEmailToGroup(email,BCC);
    }
    private void AddEmailToGroup(EmailAddress email, List<EmailAddress> emailGroup)
    {
        Guard.Against.Null(email, nameof(email));
        emailGroup.Add(email);
    }
    private void AddEmailsToGroup(List<EmailAddress> emailsToAdd, List<EmailAddress> emailGroup)
    {
        Guard.Against.Null(emailsToAdd, nameof(emailsToAdd));
        emailGroup.Union(emailsToAdd);
    }
    /// <summary>
    /// Set a global subject line.
    /// </summary>
    /// <param name="subject">The subject of your email. This may be overridden by personalizations[x].subject.</param>
    public void SetGlobalSubject(string subject)
    {
        this.Subject = subject;
    }
}
