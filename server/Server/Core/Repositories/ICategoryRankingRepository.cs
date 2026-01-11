using Server.Core.Entities.Entities;

namespace Server.Core.Repositories;

public interface ICategoryRankingRepository
{
    Task<CategoryRanking?> GetByIdAsync(Guid id);
    Task AddAsync(CategoryRanking entity);
    Task SaveAsync(CategoryRanking entity);
    Task DeleteAsync(Guid id);
}