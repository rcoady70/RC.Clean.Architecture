using System;
using RC.CA.Domain.Entities.Cdn;

namespace RC.CA.Application.Contracts.Persistence;
public interface ICdnFileRepository: IAsyncRepository<CdnFiles>
{
}

