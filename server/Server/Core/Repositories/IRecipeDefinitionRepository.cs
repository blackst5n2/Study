using Server.Core.Entities.Definitions;

namespace Server.Core.Repositories;

public interface IRecipeDefinitionRepository
{
    Task<RecipeDefinition?> GetByIdAsync(Guid id);
    Task AddAsync(RecipeDefinition entity);
    Task SaveAsync(RecipeDefinition entity);
    Task DeleteAsync(Guid id);
    Task<RecipeDefinition?> GetByCodeAsync(string code);
    Task AddByCodeAsync(RecipeDefinition entity);
    Task SaveByCodeAsync(RecipeDefinition entity);
    Task DeleteByCodeAsync(string code);
}