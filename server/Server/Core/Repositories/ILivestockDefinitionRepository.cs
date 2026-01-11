using Server.Core.Entities.Definitions;

namespace Server.Core.Repositories;

public interface ILivestockDefinitionRepository
{
    Task<LivestockDefinition?> GetByIdAsync(Guid id);
    Task AddAsync(LivestockDefinition entity);
    Task SaveAsync(LivestockDefinition entity);
    Task DeleteAsync(Guid id);
}