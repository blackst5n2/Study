using Server.Core.Entities.Definitions;

namespace Server.Core.Repositories;

public interface IJobLevelDefinitionRepository
{
    Task<JobLevelDefinition?> GetByIdAsync(Guid id);
    Task AddAsync(JobLevelDefinition entity);
    Task SaveAsync(JobLevelDefinition entity);
    Task DeleteAsync(Guid id);
}