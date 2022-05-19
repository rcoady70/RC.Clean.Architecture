
namespace RC.CA.WebUiMvc.Middleware.SecurityHeaders
{
    /// <summary>
    /// Exposes methods to build a policy.
    /// </summary>
    /// <summary>
    /// Exposes methods to build a policy.
    /// </summary>
    public class SecurityHeadersBuilder
    {
        private ISecurityHeadersDto _securityHeaders { get; set; }

        /// <summary>
        /// The number of seconds in one year
        /// </summary>
        public const int OneYearInSeconds = 60 * 60 * 24 * 365;

        /// <summary>
        /// Add default headers in accordance with most secure approach
        /// </summary>
        public SecurityHeadersBuilder AddDefaultSecurePolicy(ISecurityHeadersDto securityHeaders)
        {
            _securityHeaders = securityHeaders;
            return this;
        }
                       
        /// <summary>
        /// Builds a new <see cref="SecurityHeadersPolicy"/> using the entries added.
        /// </summary>
        /// <returns>The constructed <see cref="SecurityHeadersPolicy"/>.</returns>
        public ISecurityHeadersDto Build()
        {
            return _securityHeaders;
        }
    }
}
