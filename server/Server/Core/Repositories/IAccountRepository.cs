using Server.Core.Entities.Entities;

namespace Server.Core.Repositories;

public interface IAccountRepository
{
    Task<Account?> GetByIdAsync(Guid id);
    Task AddAsync(Account entity);
    Task SaveAsync(Account entity);
    Task DeleteAsync(Guid id);
}