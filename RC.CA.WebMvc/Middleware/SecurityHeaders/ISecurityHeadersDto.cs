namespace RC.CA.WebUiMvc.Middleware.SecurityHeaders
{
    public interface ISecurityHeadersDto
    {
        string Csp { get; set; }
        string CspReportOnly { get; set; }
        string CspReportUri { get; set; }
        string XFrameOptions { get; set; }
        string FeaturePolicy { get; set; }
        string  XContentTypeOptions { get; set; }
        string ReferrerPolicy { get; set; }
        string StrictTransportSecurity { get; set; }
        string CacheControl { get; set; }
    }

}
