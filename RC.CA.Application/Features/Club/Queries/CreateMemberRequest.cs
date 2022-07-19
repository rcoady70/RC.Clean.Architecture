using System.Text.Json.Serialization;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using RC.CA.Application.Dto.Club;

namespace RC.CA.Application.Features.Club.Queries;
/// <summary>
/// Create member request
/// </summary>
public class CreateMemberRequest : IRequest<CAResult<CreateMemberResponseDto>>
{
    public Guid? Id { get; set; }
    public string Name { get; set; } = "";
    public string Gender { get; set; } = "";
    public string Qualification { get; set; } = "";
    public virtual List<CreateExperienceRequest> Experiences { get; set; } = new List<CreateExperienceRequest>();
    public string? PhotoUrl { get; set; }
    [JsonIgnore]
    public IFormFile? ProfilePhoto { get; set; }

    [JsonIgnore]
    public List<SelectListItem> GenderListItems { get; set; } = new List<SelectListItem>() { new SelectListItem("", ""), new SelectListItem("Male", "M"), new SelectListItem("Female", "F"), new SelectListItem("Prefer not to say", "U") };

}
