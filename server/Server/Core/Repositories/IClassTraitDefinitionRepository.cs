using Server.Core.Entities.Definitions;

namespace Server.Core.Repositories;

public interface IClassTraitDefinitionRepository
{
    Task<ClassTraitDefinition?> GetByIdAsync(Guid id);
    Task AddAsync(ClassTraitDefinition entity);
    Task SaveAsync(ClassTraitDefinition entity);
    Task DeleteAsync(Guid id);
}