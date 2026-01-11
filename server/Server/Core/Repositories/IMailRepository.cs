using Server.Core.Entities.Entities;

namespace Server.Core.Repositories;

public interface IMailRepository
{
    Task<Mail?> GetByIdAsync(Guid id);
    Task AddAsync(Mail entity);
    Task SaveAsync(Mail entity);
    Task DeleteAsync(Guid id);
    Task<Mail?> GetByCodeAsync(string code);
    Task AddByCodeAsync(Mail entity);
    Task SaveByCodeAsync(Mail entity);
    Task DeleteByCodeAsync(string code);
}