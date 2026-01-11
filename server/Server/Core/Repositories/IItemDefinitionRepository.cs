using Server.Core.Entities.Definitions;

namespace Server.Core.Repositories;

public interface IItemDefinitionRepository
{
    Task<ItemDefinition?> GetByIdAsync(Guid id);
    Task AddAsync(ItemDefinition entity);
    Task SaveAsync(ItemDefinition entity);
    Task DeleteAsync(Guid id);
    Task<ItemDefinition?> GetByCodeAsync(string code);
    Task AddByCodeAsync(ItemDefinition entity);
    Task SaveByCodeAsync(ItemDefinition entity);
    Task DeleteByCodeAsync(string code);
}