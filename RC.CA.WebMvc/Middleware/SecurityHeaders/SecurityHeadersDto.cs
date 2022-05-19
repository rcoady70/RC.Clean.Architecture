namespace RC.CA.WebUiMvc.Middleware.SecurityHeaders;

public class SecurityHeadersDto : ISecurityHeadersDto
{
    private string _csp;
    private string _cspReportOnly;

    public string Csp
    {
        get
        {
            if (!string.IsNullOrEmpty(_csp))
            {
                if (!_csp.Trim().EndsWith(";"))
                    _csp = _csp.Trim() + ";";
            }
            return _csp;
        }
        set { _csp = value; }
    }
    public string CspReportOnly
    {
        get
        {
            if (!string.IsNullOrEmpty(_cspReportOnly))
            {
                if (!_cspReportOnly.Trim().EndsWith(";"))
                    _cspReportOnly = _cspReportOnly.Trim() + ";";
            }
            return _cspReportOnly;
        }
        set { _cspReportOnly = value; }
    }

    public string CspReportUri { get; set; } = default!;
    public string XFrameOptions { get; set; } = default!;
    public string FeaturePolicy { get; set; } = default!;
    public string XContentTypeOptions { get; set; } = default!;
    public string ReferrerPolicy { get; set; } = default!;
    public string StrictTransportSecurity { get; set; } = default!;
    public string CacheControl { get; set; } = default!;
}
