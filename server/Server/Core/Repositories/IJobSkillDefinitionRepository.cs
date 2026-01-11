using Server.Core.Entities.Definitions;

namespace Server.Core.Repositories;

public interface IJobSkillDefinitionRepository
{
    Task<JobSkillDefinition?> GetByIdAsync(Guid id);
    Task AddAsync(JobSkillDefinition entity);
    Task SaveAsync(JobSkillDefinition entity);
    Task DeleteAsync(Guid id);
    Task<JobSkillDefinition?> GetByCodeAsync(string code);
    Task AddByCodeAsync(JobSkillDefinition entity);
    Task SaveByCodeAsync(JobSkillDefinition entity);
    Task DeleteByCodeAsync(string code);
}