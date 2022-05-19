using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RC.CA.Application.Contracts.Persistence;
public interface IUnitOfWork 
{
    IMemberRepository MemberRepository { get; }
    IExperienceRepository ExperienceRepository { get; }
    ICdnFileRepository CdnFileRepository { get; }
    Task SaveChangesAsync();
}
