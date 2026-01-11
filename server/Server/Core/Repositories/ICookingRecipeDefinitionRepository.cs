using Server.Core.Entities.Definitions;

namespace Server.Core.Repositories;

public interface ICookingRecipeDefinitionRepository
{
    Task<CookingRecipeDefinition?> GetByIdAsync(Guid id);
    Task AddAsync(CookingRecipeDefinition entity);
    Task SaveAsync(CookingRecipeDefinition entity);
    Task DeleteAsync(Guid id);
    Task<CookingRecipeDefinition?> GetByCodeAsync(string code);
    Task AddByCodeAsync(CookingRecipeDefinition entity);
    Task SaveByCodeAsync(CookingRecipeDefinition entity);
    Task DeleteByCodeAsync(string code);
}