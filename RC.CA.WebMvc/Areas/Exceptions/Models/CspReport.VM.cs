using System.Text.Json.Serialization;

namespace RC.CA.WebUiMvc.Areas.Exceptions.Models;

public class CspReportDto
{
    [JsonPropertyName("document-uri")]
    public string DocumentUri { get; set; } = default!;

    [JsonPropertyName("referrer")]
    public string Referrer { get; set; } = default!;

    [JsonPropertyName("violated-directive")]
    public string ViolatedDirective { get; set; } = default!;

    [JsonPropertyName("effective-directive")]
    public string EffectiveDirective { get; set; } = default!;

    [JsonPropertyName("original-policy")]
    public string OriginalPolicy { get; set; } = default!;

    [JsonPropertyName("disposition")]
    public string Disposition { get; set; } = default!;

    [JsonPropertyName("blocked-uri")]
    public string BlockedUri { get; set; } = default!;

    [JsonPropertyName("line-number")]
    public int Linenumber { get; set; } = default!;

    [JsonPropertyName("source-file")]
    public string Sourcefile { get; set; } = default!;

    [JsonPropertyName("status-code")]
    public int StatusCode { get; set; } = default!;
    [JsonPropertyName("script-sample")]
    public string ScriptSample { get; set; } = default!;
}
