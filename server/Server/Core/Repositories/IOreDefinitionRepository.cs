using Server.Core.Entities.Definitions;

namespace Server.Core.Repositories;

public interface IOreDefinitionRepository
{
    Task<OreDefinition?> GetByIdAsync(Guid id);
    Task AddAsync(OreDefinition entity);
    Task SaveAsync(OreDefinition entity);
    Task DeleteAsync(Guid id);
}