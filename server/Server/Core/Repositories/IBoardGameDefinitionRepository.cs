using Server.Core.Entities.Definitions;

namespace Server.Core.Repositories;

public interface IBoardGameDefinitionRepository
{
    Task<BoardGameDefinition?> GetByIdAsync(Guid id);
    Task AddAsync(BoardGameDefinition entity);
    Task SaveAsync(BoardGameDefinition entity);
    Task DeleteAsync(Guid id);
    Task<BoardGameDefinition?> GetByCodeAsync(string code);
    Task AddByCodeAsync(BoardGameDefinition entity);
    Task SaveByCodeAsync(BoardGameDefinition entity);
    Task DeleteByCodeAsync(string code);
}