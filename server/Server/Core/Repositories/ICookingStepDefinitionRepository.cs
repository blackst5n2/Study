using Server.Core.Entities.Definitions;

namespace Server.Core.Repositories;

public interface ICookingStepDefinitionRepository
{
    Task<CookingStepDefinition?> GetByIdAsync(Guid id);
    Task AddAsync(CookingStepDefinition entity);
    Task SaveAsync(CookingStepDefinition entity);
    Task DeleteAsync(Guid id);
}