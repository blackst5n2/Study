using Server.Core.Entities.Definitions;

namespace Server.Core.Repositories;

public interface IBuildingDefinitionRepository
{
    Task<BuildingDefinition?> GetByIdAsync(Guid id);
    Task AddAsync(BuildingDefinition entity);
    Task SaveAsync(BuildingDefinition entity);
    Task DeleteAsync(Guid id);
    Task<BuildingDefinition?> GetByCodeAsync(string code);
    Task AddByCodeAsync(BuildingDefinition entity);
    Task SaveByCodeAsync(BuildingDefinition entity);
    Task DeleteByCodeAsync(string code);
}