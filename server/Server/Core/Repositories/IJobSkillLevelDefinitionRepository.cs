using Server.Core.Entities.Definitions;

namespace Server.Core.Repositories;

public interface IJobSkillLevelDefinitionRepository
{
    Task<JobSkillLevelDefinition?> GetByIdAsync(Guid id);
    Task AddAsync(JobSkillLevelDefinition entity);
    Task SaveAsync(JobSkillLevelDefinition entity);
    Task DeleteAsync(Guid id);
}