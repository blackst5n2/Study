using Server.Core.Entities.Definitions;

namespace Server.Core.Repositories;

public interface IClassSkillDefinitionRepository
{
    Task<ClassSkillDefinition?> GetByIdAsync(Guid id);
    Task AddAsync(ClassSkillDefinition entity);
    Task SaveAsync(ClassSkillDefinition entity);
    Task DeleteAsync(Guid id);
}