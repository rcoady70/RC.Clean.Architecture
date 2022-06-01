using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RC.CA.Application.Dto.Cdn
{
    /// <summary>
    /// Column mapping 
    /// </summary>
    public class CsvColumnMapDto 
    {
        public string FromCsvField { get; set; } = "";
        public string? SampleData { get; set; } = "";
        public string? ToEntityField { get; set; } = "";
        public string? Mask { get; set; } = "";
        public string? Macro { get; set; } = "";
        [JsonIgnore]
        public IEnumerable<SelectListItem> ToEntityFieldListItems { get; set; } = WebConstants.MembersImportFields.Select(i => new SelectListItem() { Text = i.Key, Value = i.Value });
    }
}
