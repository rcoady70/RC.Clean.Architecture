using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RC.CA.Domain.Entities.Club;
using RC.CA.Application.Models;

namespace RC.CA.Application.Dto.Club;
public class GetMemberResponseDto : BaseResponseDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = "";

    public string Gender { get; set; } = "";
    public string PhotoUrl { get; set; } = "";

    public string Qualification { get; set; } = "";

    public List<GetMemberExperienceDto> Experiences { get; set; } = new List<GetMemberExperienceDto>();
}

