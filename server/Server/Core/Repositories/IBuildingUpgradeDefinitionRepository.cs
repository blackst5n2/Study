using Server.Core.Entities.Definitions;

namespace Server.Core.Repositories;

public interface IBuildingUpgradeDefinitionRepository
{
    Task<BuildingUpgradeDefinition?> GetByIdAsync(Guid id);
    Task AddAsync(BuildingUpgradeDefinition entity);
    Task SaveAsync(BuildingUpgradeDefinition entity);
    Task DeleteAsync(Guid id);
}