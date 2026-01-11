using Server.Core.Entities.Definitions;

namespace Server.Core.Repositories;

public interface ILogDefinitionRepository
{
    Task<LogDefinition?> GetByIdAsync(Guid id);
    Task AddAsync(LogDefinition entity);
    Task SaveAsync(LogDefinition entity);
    Task DeleteAsync(Guid id);
}