using Server.Core.Entities.Definitions;

namespace Server.Core.Repositories;

public interface IJobDefinitionRepository
{
    Task<JobDefinition?> GetByIdAsync(Guid id);
    Task AddAsync(JobDefinition entity);
    Task SaveAsync(JobDefinition entity);
    Task DeleteAsync(Guid id);
    Task<JobDefinition?> GetByCodeAsync(string code);
    Task AddByCodeAsync(JobDefinition entity);
    Task SaveByCodeAsync(JobDefinition entity);
    Task DeleteByCodeAsync(string code);
}