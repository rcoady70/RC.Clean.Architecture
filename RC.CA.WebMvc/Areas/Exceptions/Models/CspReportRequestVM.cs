using System.Text.Json.Serialization;

namespace RC.CA.WebUiMvc.Areas.Exceptions.Models;

public class CspReportRequestVM
{
    [JsonPropertyName("csp-report")]
    public CspReportDto cspreport { get; set; }
}

