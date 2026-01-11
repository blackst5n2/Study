using Server.Core.Entities.Entities;

namespace Server.Core.Repositories;

public interface IAuthRepository
{
    Task<Auth?> GetByIdAsync(Guid id);
    Task AddAsync(Auth entity);
    Task SaveAsync(Auth entity);
    Task DeleteAsync(Guid id);
}