using Server.Core.Entities.Definitions;

namespace Server.Core.Repositories;

public interface IDropTableDefinitionRepository
{
    Task<DropTableDefinition?> GetByIdAsync(Guid id);
    Task AddAsync(DropTableDefinition entity);
    Task SaveAsync(DropTableDefinition entity);
    Task DeleteAsync(Guid id);
    Task<DropTableDefinition?> GetByCodeAsync(string code);
    Task AddByCodeAsync(DropTableDefinition entity);
    Task SaveByCodeAsync(DropTableDefinition entity);
    Task DeleteByCodeAsync(string code);
}