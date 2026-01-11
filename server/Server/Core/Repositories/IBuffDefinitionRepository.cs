using Server.Core.Entities.Definitions;

namespace Server.Core.Repositories;

public interface IBuffDefinitionRepository
{
    Task<BuffDefinition?> GetByIdAsync(Guid id);
    Task AddAsync(BuffDefinition entity);
    Task SaveAsync(BuffDefinition entity);
    Task DeleteAsync(Guid id);
    Task<BuffDefinition?> GetByCodeAsync(string code);
    Task AddByCodeAsync(BuffDefinition entity);
    Task SaveByCodeAsync(BuffDefinition entity);
    Task DeleteByCodeAsync(string code);
}