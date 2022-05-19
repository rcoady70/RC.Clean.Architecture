using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RC.CA.Application.Contracts.Persistence;
using RC.CA.Domain.Entities.Club;

namespace RC.CA.Infrastructure.Persistence.Specifications;
public class MemberFilterPaginatedSpecification: SpecificationBase<Member>
{
    public MemberFilterPaginatedSpecification(int skip, int take) : base()
    {
        this.ApplyOrderBy(o => o.Id);
    }
}
