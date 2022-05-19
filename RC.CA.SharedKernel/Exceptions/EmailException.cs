using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RC.CA.SharedKernel.Exceptions
{
    public partial class EmailException : System.Exception
    {
        public string From { get; private set; }
        public string To { get; private set; }
        public string Subject { get; private set; }
        public string Provider { get; private set; }

        public EmailException(string from, string to,string subject,string provider, System.Exception? innerException)
                            : base($"Email send failed \nfrom: {from} to: {to} subject: {subject} provider: {provider} \n")
        {
            From = from;
            To = to;
            Subject = subject;
            Provider = provider;
        }
    }
}
