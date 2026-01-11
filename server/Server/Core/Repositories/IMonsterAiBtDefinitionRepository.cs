using Server.Core.Entities.Definitions;

namespace Server.Core.Repositories;

public interface IMonsterAiBtDefinitionRepository
{
    Task<MonsterAiBtDefinition?> GetByIdAsync(Guid id);
    Task AddAsync(MonsterAiBtDefinition entity);
    Task SaveAsync(MonsterAiBtDefinition entity);
    Task DeleteAsync(Guid id);
    Task<MonsterAiBtDefinition?> GetByCodeAsync(string code);
    Task AddByCodeAsync(MonsterAiBtDefinition entity);
    Task SaveByCodeAsync(MonsterAiBtDefinition entity);
    Task DeleteByCodeAsync(string code);
}