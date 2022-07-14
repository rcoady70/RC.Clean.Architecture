using RC.CA.Application.Models;

namespace RC.CA.Application.Dto.Club;
public class GetMemberResponseDto : BaseResponseCAResult
{
    public Guid Id { get; set; }

    public string Name { get; set; } = "";

    public string Gender { get; set; } = "";
    public string PhotoUrl { get; set; } = "";

    public string Qualification { get; set; } = "";

    public List<GetMemberExperienceDto> Experiences { get; set; } = new List<GetMemberExperienceDto>();
}

