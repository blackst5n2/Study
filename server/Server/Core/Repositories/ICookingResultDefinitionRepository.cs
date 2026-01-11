using Server.Core.Entities.Definitions;

namespace Server.Core.Repositories;

public interface ICookingResultDefinitionRepository
{
    Task<CookingResultDefinition?> GetByIdAsync(Guid id);
    Task AddAsync(CookingResultDefinition entity);
    Task SaveAsync(CookingResultDefinition entity);
    Task DeleteAsync(Guid id);
}