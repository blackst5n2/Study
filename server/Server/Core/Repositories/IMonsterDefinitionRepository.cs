using Server.Core.Entities.Definitions;

namespace Server.Core.Repositories;

public interface IMonsterDefinitionRepository
{
    Task<MonsterDefinition?> GetByIdAsync(Guid id);
    Task AddAsync(MonsterDefinition entity);
    Task SaveAsync(MonsterDefinition entity);
    Task DeleteAsync(Guid id);
}