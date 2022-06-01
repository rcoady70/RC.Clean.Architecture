using RC.CA.Domain.Entities.Shared;

namespace RC.CA.Domain.Entities.Club;
public class Experience: BaseEntity<int>
{
    public Guid MemberId { get; set; }
    public string QualificationName { get; set; } = "";
    public string Description { get; set; } = "";
    public DateTime ExpiryDate { get; set; }
}
