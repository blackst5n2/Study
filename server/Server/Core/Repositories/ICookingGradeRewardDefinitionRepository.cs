using Server.Core.Entities.Definitions;

namespace Server.Core.Repositories;

public interface ICookingGradeRewardDefinitionRepository
{
    Task<CookingGradeRewardDefinition?> GetByIdAsync(Guid id);
    Task AddAsync(CookingGradeRewardDefinition entity);
    Task SaveAsync(CookingGradeRewardDefinition entity);
    Task DeleteAsync(Guid id);
}