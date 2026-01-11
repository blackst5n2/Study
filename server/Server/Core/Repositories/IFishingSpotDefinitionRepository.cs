using Server.Core.Entities.Definitions;

namespace Server.Core.Repositories;

public interface IFishingSpotDefinitionRepository
{
    Task<FishingSpotDefinition?> GetByIdAsync(Guid id);
    Task AddAsync(FishingSpotDefinition entity);
    Task SaveAsync(FishingSpotDefinition entity);
    Task DeleteAsync(Guid id);
    Task<FishingSpotDefinition?> GetByCodeAsync(string code);
    Task AddByCodeAsync(FishingSpotDefinition entity);
    Task SaveByCodeAsync(FishingSpotDefinition entity);
    Task DeleteByCodeAsync(string code);
}