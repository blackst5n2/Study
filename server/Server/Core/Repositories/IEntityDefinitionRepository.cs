using Server.Core.Entities.Definitions;

namespace Server.Core.Repositories;

public interface IEntityDefinitionRepository
{
    Task<EntityDefinition?> GetByIdAsync(Guid id);
    Task AddAsync(EntityDefinition entity);
    Task SaveAsync(EntityDefinition entity);
    Task DeleteAsync(Guid id);
    Task<EntityDefinition?> GetByCodeAsync(string code);
    Task AddByCodeAsync(EntityDefinition entity);
    Task SaveByCodeAsync(EntityDefinition entity);
    Task DeleteByCodeAsync(string code);
}