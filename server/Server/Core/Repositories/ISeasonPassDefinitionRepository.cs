using Server.Core.Entities.Definitions;

namespace Server.Core.Repositories;

public interface ISeasonPassDefinitionRepository
{
    Task<SeasonPassDefinition?> GetByIdAsync(Guid id);
    Task AddAsync(SeasonPassDefinition entity);
    Task SaveAsync(SeasonPassDefinition entity);
    Task DeleteAsync(Guid id);
    Task<SeasonPassDefinition?> GetByCodeAsync(string code);
    Task AddByCodeAsync(SeasonPassDefinition entity);
    Task SaveByCodeAsync(SeasonPassDefinition entity);
    Task DeleteByCodeAsync(string code);
}