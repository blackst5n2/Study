using Server.Core.Entities.Definitions;

namespace Server.Core.Repositories;

public interface IMiniGameDefinitionRepository
{
    Task<MiniGameDefinition?> GetByIdAsync(Guid id);
    Task AddAsync(MiniGameDefinition entity);
    Task SaveAsync(MiniGameDefinition entity);
    Task DeleteAsync(Guid id);
    Task<MiniGameDefinition?> GetByCodeAsync(string code);
    Task AddByCodeAsync(MiniGameDefinition entity);
    Task SaveByCodeAsync(MiniGameDefinition entity);
    Task DeleteByCodeAsync(string code);
}