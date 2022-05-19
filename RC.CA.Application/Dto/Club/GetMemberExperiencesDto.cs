using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RC.CA.Domain.Entities.Common;

namespace RC.CA.Application.Dto.Club;
public class GetMemberExperienceDto : BaseEntity<int>
{
    public Guid MemberId { get; set; }
    public string QualificationName { get; set; } = "";
    public string Description { get; set; } = "";
    public DateTime ExpiryDate { get; set; } = DateTime.Now;
}
