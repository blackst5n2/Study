using Server.Core.Entities.Definitions;

namespace Server.Core.Repositories;

public interface IEventDefinitionRepository
{
    Task<EventDefinition?> GetByIdAsync(Guid id);
    Task AddAsync(EventDefinition entity);
    Task SaveAsync(EventDefinition entity);
    Task DeleteAsync(Guid id);
    Task<EventDefinition?> GetByCodeAsync(string code);
    Task AddByCodeAsync(EventDefinition entity);
    Task SaveByCodeAsync(EventDefinition entity);
    Task DeleteByCodeAsync(string code);
}