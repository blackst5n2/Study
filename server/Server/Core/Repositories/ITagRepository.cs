using Server.Core.Entities.Entities;

namespace Server.Core.Repositories;

public interface ITagRepository
{
    Task<Tag?> GetByIdAsync(Guid id);
    Task AddAsync(Tag entity);
    Task SaveAsync(Tag entity);
    Task DeleteAsync(Guid id);
    Task<Tag?> GetByCodeAsync(string code);
    Task AddByCodeAsync(Tag entity);
    Task SaveByCodeAsync(Tag entity);
    Task DeleteByCodeAsync(string code);
}