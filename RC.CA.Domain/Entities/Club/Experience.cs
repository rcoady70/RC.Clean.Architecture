using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RC.CA.Domain.Entities.Common;

namespace RC.CA.Domain.Entities.Club;
public class Experience: BaseEntity<int>
{
    public Guid MemberId { get; set; }
    public string QualificationName { get; set; } = "";
    public string Description { get; set; } = "";
    public DateTime ExpiryDate { get; set; }
}
