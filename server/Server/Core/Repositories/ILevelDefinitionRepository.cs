using Server.Core.Entities.Definitions;

namespace Server.Core.Repositories;

public interface ILevelDefinitionRepository
{
    Task<LevelDefinition?> GetByIdAsync(Guid id);
    Task AddAsync(LevelDefinition entity);
    Task SaveAsync(LevelDefinition entity);
    Task DeleteAsync(Guid id);
    Task<LevelDefinition?> GetByCodeAsync(string code);
    Task AddByCodeAsync(LevelDefinition entity);
    Task SaveByCodeAsync(LevelDefinition entity);
    Task DeleteByCodeAsync(string code);
}