using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RC.CA.Application.Features.Club.Queries;

public class CreateExperienceRequest
{
    public int Id { get; set; }
    public Guid MemberId { get; set; }
    public string QualificationName { get; set; }
    public string Description { get; set; }
    public DateTime ExpiryDate { get; set; } = DateTime.Now;
    [JsonIgnore]
    List<SelectListItem> GenderListItems = new List<SelectListItem>() { new SelectListItem("", ""), new SelectListItem("Male", "M"), new SelectListItem("Female", "F"), new SelectListItem("Prefer not to say", "U") };
}
