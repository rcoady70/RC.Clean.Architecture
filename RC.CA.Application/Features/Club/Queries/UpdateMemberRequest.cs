using MediatR;
using RC.CA.Application.Dto.Club;

namespace RC.CA.Application.Features.Club.Queries;
/// <summary>
/// Create member request
/// </summary>
public class UpdateMemberRequest : IRequest<CAResult<CreateMemberResponseDto>>
{
    public Guid? Id { get; set; }
    public string Name { get; set; } = "";
    public string Gender { get; set; } = "";
    public string Qualification { get; set; } = "";
    public virtual List<CreateExperienceRequest> Experiences { get; set; } = new List<CreateExperienceRequest>();
    public string? PhotoUrl { get; set; }

}
