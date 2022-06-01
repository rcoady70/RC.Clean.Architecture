namespace RC.CA.Domain.Entities.CSV;

/// <summary>
/// Mapping line 
/// </summary>
public class CsvMapColumn 
{
    public CsvMapColumn(string fromCsvField,string toCsvFiled, string mask="", string macro="",string sampleData="")
    {
        FromCsvField = fromCsvField;
        SampleData = sampleData;
        ToCsvFiled = toCsvFiled;
        Mask = mask;
        Macro = macro;
    }
    public string FromCsvField { get; set; } = "";
    public string ToCsvFiled { get; set; } = "";
    public string SampleData { get; set; } = "";
    public string Mask { get; set; } = "";
    public string Macro { get; set; } = "";
}
