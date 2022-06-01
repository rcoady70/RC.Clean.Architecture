namespace RC.CA.Domain.Entities.CSV;
/// <summary>
/// CSV mapping 
/// </summary>
public class CsvMap
{
    public CsvMap(Guid id)
    {
        this.Id = id;
    }
    public Guid Id { get; set; } = default;
    public List<CsvMapColumn> MapColumns { get; set; } = new List<CsvMapColumn>();
   
}
