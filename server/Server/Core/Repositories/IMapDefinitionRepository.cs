using Server.Core.Entities.Definitions;

namespace Server.Core.Repositories;

public interface IMapDefinitionRepository
{
    Task<MapDefinition?> GetByIdAsync(Guid id);
    Task AddAsync(MapDefinition entity);
    Task SaveAsync(MapDefinition entity);
    Task DeleteAsync(Guid id);
    Task<MapDefinition?> GetByCodeAsync(string code);
    Task AddByCodeAsync(MapDefinition entity);
    Task SaveByCodeAsync(MapDefinition entity);
    Task DeleteByCodeAsync(string code);
}