using Server.Core.Entities.Entities;

namespace Server.Core.Repositories;

public interface IAchievementCategoryRepository
{
    Task<AchievementCategory?> GetByIdAsync(Guid id);
    Task AddAsync(AchievementCategory entity);
    Task SaveAsync(AchievementCategory entity);
    Task DeleteAsync(Guid id);
    Task<AchievementCategory?> GetByCodeAsync(string code);
    Task AddByCodeAsync(AchievementCategory entity);
    Task SaveByCodeAsync(AchievementCategory entity);
    Task DeleteByCodeAsync(string code);
}