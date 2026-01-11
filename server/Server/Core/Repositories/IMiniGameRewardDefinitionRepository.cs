using Server.Core.Entities.Definitions;

namespace Server.Core.Repositories;

public interface IMiniGameRewardDefinitionRepository
{
    Task<MiniGameRewardDefinition?> GetByIdAsync(Guid id);
    Task AddAsync(MiniGameRewardDefinition entity);
    Task SaveAsync(MiniGameRewardDefinition entity);
    Task DeleteAsync(Guid id);
}