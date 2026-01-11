using Server.Core.Entities.Definitions;

namespace Server.Core.Repositories;

public interface IClassDefinitionRepository
{
    Task<ClassDefinition?> GetByIdAsync(Guid id);
    Task AddAsync(ClassDefinition entity);
    Task SaveAsync(ClassDefinition entity);
    Task DeleteAsync(Guid id);
    Task<ClassDefinition?> GetByCodeAsync(string code);
    Task AddByCodeAsync(ClassDefinition entity);
    Task SaveByCodeAsync(ClassDefinition entity);
    Task DeleteByCodeAsync(string code);
}