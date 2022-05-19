using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RC.CA.SharedKernel.WebHelpers;
/// <summary>
/// Generate per request id's
/// </summary>
public interface INonce
{
    // Nouce used in CSP policy
    string CspNonce { get;  }
    // Used to add CorrelationId header to tie back and front end requests together 
    string CorrelationId { get;  }
}
/// <summary>
/// Generate per request unigue id's
/// </summary>
public class Nonce : INonce
{
    //Content security provider nouce
    public string CspNonce { get; private set; }
    // Used to add CorrelationId header to tie back and front end requests together 
    public string CorrelationId { get; private set; }

    public Nonce()
    {
        if (string.IsNullOrEmpty(CspNonce))
            this.CspNonce = Guid.NewGuid().ToString().Replace("-", "");
        if (string.IsNullOrEmpty(CorrelationId))
            this.CorrelationId = "cid_" + Guid.NewGuid().ToString().Replace("-", "");
    }

}
