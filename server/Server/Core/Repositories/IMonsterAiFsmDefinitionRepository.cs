using Server.Core.Entities.Definitions;

namespace Server.Core.Repositories;

public interface IMonsterAiFsmDefinitionRepository
{
    Task<MonsterAiFsmDefinition?> GetByIdAsync(Guid id);
    Task AddAsync(MonsterAiFsmDefinition entity);
    Task SaveAsync(MonsterAiFsmDefinition entity);
    Task DeleteAsync(Guid id);
    Task<MonsterAiFsmDefinition?> GetByCodeAsync(string code);
    Task AddByCodeAsync(MonsterAiFsmDefinition entity);
    Task SaveByCodeAsync(MonsterAiFsmDefinition entity);
    Task DeleteByCodeAsync(string code);
}