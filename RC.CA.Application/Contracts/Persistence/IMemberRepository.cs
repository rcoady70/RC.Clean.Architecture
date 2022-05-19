using System;
using RC.CA.Domain.Entities.Club;

namespace RC.CA.Application.Contracts.Persistence;
public interface IMemberRepository: IAsyncRepository<Member>
{
}
