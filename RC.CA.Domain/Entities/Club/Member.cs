using RC.CA.Domain.Entities.Shared;

namespace RC.CA.Domain.Entities.Club;
/// <summary>
/// Club member
/// </summary>
public class Member : BaseEntity<Guid>
{
    public string Name { get; set; } = "";

    public string Gender { get; set; } = "";

    public string Qualification { get; set; } = "";
    //Virtual
    public virtual List<Experience> Experiences { get; set; } = new List<Experience>();

    public string? PhotoUrl { get; set; }


}
