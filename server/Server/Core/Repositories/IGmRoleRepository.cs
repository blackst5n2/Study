using Server.Core.Entities.Entities;

namespace Server.Core.Repositories;

public interface IGmRoleRepository
{
    Task<GmRole?> GetByIdAsync(Guid id);
    Task AddAsync(GmRole entity);
    Task SaveAsync(GmRole entity);
    Task DeleteAsync(Guid id);
    Task<GmRole?> GetByCodeAsync(string code);
    Task AddByCodeAsync(GmRole entity);
    Task SaveByCodeAsync(GmRole entity);
    Task DeleteByCodeAsync(string code);
}