using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RC.CA.Application.Contracts.Persistence;
using RC.CA.Domain.Entities.Club;

namespace RC.CA.Infrastructure.Persistence.Repository;
public class ExperienceRepository: AsyncRepository<Experience>, IExperienceRepository
{
    private readonly ApplicationDbContext _dbContext;
    public ExperienceRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
