using Server.Core.Entities.Definitions;

namespace Server.Core.Repositories;

public interface IDungeonDefinitionRepository
{
    Task<DungeonDefinition?> GetByIdAsync(Guid id);
    Task AddAsync(DungeonDefinition entity);
    Task SaveAsync(DungeonDefinition entity);
    Task DeleteAsync(Guid id);
    Task<DungeonDefinition?> GetByCodeAsync(string code);
    Task AddByCodeAsync(DungeonDefinition entity);
    Task SaveByCodeAsync(DungeonDefinition entity);
    Task DeleteByCodeAsync(string code);
}