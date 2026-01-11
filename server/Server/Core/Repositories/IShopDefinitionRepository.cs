using Server.Core.Entities.Definitions;

namespace Server.Core.Repositories;

public interface IShopDefinitionRepository
{
    Task<ShopDefinition?> GetByIdAsync(Guid id);
    Task AddAsync(ShopDefinition entity);
    Task SaveAsync(ShopDefinition entity);
    Task DeleteAsync(Guid id);
    Task<ShopDefinition?> GetByCodeAsync(string code);
    Task AddByCodeAsync(ShopDefinition entity);
    Task SaveByCodeAsync(ShopDefinition entity);
    Task DeleteByCodeAsync(string code);
}