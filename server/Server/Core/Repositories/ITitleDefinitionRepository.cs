using Server.Core.Entities.Definitions;

namespace Server.Core.Repositories;

public interface ITitleDefinitionRepository
{
    Task<TitleDefinition?> GetByIdAsync(Guid id);
    Task AddAsync(TitleDefinition entity);
    Task SaveAsync(TitleDefinition entity);
    Task DeleteAsync(Guid id);
    Task<TitleDefinition?> GetByCodeAsync(string code);
    Task AddByCodeAsync(TitleDefinition entity);
    Task SaveByCodeAsync(TitleDefinition entity);
    Task DeleteByCodeAsync(string code);
}