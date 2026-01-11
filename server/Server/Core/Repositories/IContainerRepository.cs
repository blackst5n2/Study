using Server.Core.Entities.Entities;

namespace Server.Core.Repositories;

public interface IContainerRepository
{
    Task<Container?> GetByIdAsync(Guid id);
    Task AddAsync(Container entity);
    Task SaveAsync(Container entity);
    Task DeleteAsync(Guid id);
}