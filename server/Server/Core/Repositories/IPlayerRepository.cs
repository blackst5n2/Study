using Server.Core.Entities.Progress;

namespace Server.Core.Repositories;

public interface IPlayerRepository
{
    Task<Player?> GetByIdAsync(Guid id);
    Task AddAsync(Player entity);
    Task SaveAsync(Player entity);
    Task DeleteAsync(Guid id);
    Task<Player?> GetByCodeAsync(string code);
    Task AddByCodeAsync(Player entity);
    Task SaveByCodeAsync(Player entity);
    Task DeleteByCodeAsync(string code);
}