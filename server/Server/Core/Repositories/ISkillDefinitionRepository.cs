using Server.Core.Entities.Definitions;

namespace Server.Core.Repositories;

public interface ISkillDefinitionRepository
{
    Task<SkillDefinition?> GetByIdAsync(Guid id);
    Task AddAsync(SkillDefinition entity);
    Task SaveAsync(SkillDefinition entity);
    Task DeleteAsync(Guid id);
    Task<SkillDefinition?> GetByCodeAsync(string code);
    Task AddByCodeAsync(SkillDefinition entity);
    Task SaveByCodeAsync(SkillDefinition entity);
    Task DeleteByCodeAsync(string code);
}