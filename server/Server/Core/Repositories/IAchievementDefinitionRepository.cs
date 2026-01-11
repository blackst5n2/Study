using Server.Core.Entities.Definitions;

namespace Server.Core.Repositories;

public interface IAchievementDefinitionRepository
{
    Task<AchievementDefinition?> GetByIdAsync(Guid id);
    Task AddAsync(AchievementDefinition entity);
    Task SaveAsync(AchievementDefinition entity);
    Task DeleteAsync(Guid id);
    Task<AchievementDefinition?> GetByCodeAsync(string code);
    Task AddByCodeAsync(AchievementDefinition entity);
    Task SaveByCodeAsync(AchievementDefinition entity);
    Task DeleteByCodeAsync(string code);
}