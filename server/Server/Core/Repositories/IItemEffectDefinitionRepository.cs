using Server.Core.Entities.Definitions;

namespace Server.Core.Repositories;

public interface IItemEffectDefinitionRepository
{
    Task<ItemEffectDefinition?> GetByIdAsync(Guid id);
    Task AddAsync(ItemEffectDefinition entity);
    Task SaveAsync(ItemEffectDefinition entity);
    Task DeleteAsync(Guid id);
    Task<ItemEffectDefinition?> GetByCodeAsync(string code);
    Task AddByCodeAsync(ItemEffectDefinition entity);
    Task SaveByCodeAsync(ItemEffectDefinition entity);
    Task DeleteByCodeAsync(string code);
}