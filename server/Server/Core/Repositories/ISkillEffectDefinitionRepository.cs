using Server.Core.Entities.Definitions;

namespace Server.Core.Repositories;

public interface ISkillEffectDefinitionRepository
{
    Task<SkillEffectDefinition?> GetByIdAsync(Guid id);
    Task AddAsync(SkillEffectDefinition entity);
    Task SaveAsync(SkillEffectDefinition entity);
    Task DeleteAsync(Guid id);
}