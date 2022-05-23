using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using RC.CA.SharedKernel.WebHelpers;

namespace RC.CA.WebMvc.TagHelpers
{
    /// <summary>
    /// Add nonce to script link tags
    /// </summary>
    [HtmlTargetElement("script",Attributes = "rc-nonce")]
    [HtmlTargetElement("link", Attributes = "rc-nonce")]
    public class RcNonceTagHelper : TagHelper
    {
        private readonly INonce _nonce;

        public RcNonceTagHelper(INonce nonce)
        {
            _nonce = nonce;
        }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            //var rm = output.Attributes.RemoveAll("nonce");
            output.Attributes.Add("nonce", _nonce.CspNonce);
            output.Attributes.RemoveAll("rc-nonce");
        }
    }
}
