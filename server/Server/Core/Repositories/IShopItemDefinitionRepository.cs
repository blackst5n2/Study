using Server.Core.Entities.Definitions;

namespace Server.Core.Repositories;

public interface IShopItemDefinitionRepository
{
    Task<ShopItemDefinition?> GetByIdAsync(Guid id);
    Task AddAsync(ShopItemDefinition entity);
    Task SaveAsync(ShopItemDefinition entity);
    Task DeleteAsync(Guid id);
}