using Server.Core.Entities.Entities;

namespace Server.Core.Repositories;

public interface ITitleCategoryRepository
{
    Task<TitleCategory?> GetByIdAsync(Guid id);
    Task AddAsync(TitleCategory entity);
    Task SaveAsync(TitleCategory entity);
    Task DeleteAsync(Guid id);
    Task<TitleCategory?> GetByCodeAsync(string code);
    Task AddByCodeAsync(TitleCategory entity);
    Task SaveByCodeAsync(TitleCategory entity);
    Task DeleteByCodeAsync(string code);
}