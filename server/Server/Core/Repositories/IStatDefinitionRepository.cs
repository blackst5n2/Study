using Server.Core.Entities.Definitions;

namespace Server.Core.Repositories;

public interface IStatDefinitionRepository
{
    Task<StatDefinition?> GetByIdAsync(Guid id);
    Task AddAsync(StatDefinition entity);
    Task SaveAsync(StatDefinition entity);
    Task DeleteAsync(Guid id);
    Task<StatDefinition?> GetByCodeAsync(string code);
    Task AddByCodeAsync(StatDefinition entity);
    Task SaveByCodeAsync(StatDefinition entity);
    Task DeleteByCodeAsync(string code);
}