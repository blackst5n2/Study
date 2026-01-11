using Server.Core.Entities.Definitions;

namespace Server.Core.Repositories;

public interface IFishDefinitionRepository
{
    Task<FishDefinition?> GetByIdAsync(Guid id);
    Task AddAsync(FishDefinition entity);
    Task SaveAsync(FishDefinition entity);
    Task DeleteAsync(Guid id);
    Task<FishDefinition?> GetByCodeAsync(string code);
    Task AddByCodeAsync(FishDefinition entity);
    Task SaveByCodeAsync(FishDefinition entity);
    Task DeleteByCodeAsync(string code);
}