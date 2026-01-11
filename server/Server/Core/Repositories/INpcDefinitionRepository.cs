using Server.Core.Entities.Definitions;

namespace Server.Core.Repositories;

public interface INpcDefinitionRepository
{
    Task<NpcDefinition?> GetByIdAsync(Guid id);
    Task AddAsync(NpcDefinition entity);
    Task SaveAsync(NpcDefinition entity);
    Task DeleteAsync(Guid id);
    Task<NpcDefinition?> GetByCodeAsync(string code);
    Task AddByCodeAsync(NpcDefinition entity);
    Task SaveByCodeAsync(NpcDefinition entity);
    Task DeleteByCodeAsync(string code);
}