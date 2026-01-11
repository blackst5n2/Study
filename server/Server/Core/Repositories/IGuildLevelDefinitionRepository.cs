using Server.Core.Entities.Definitions;

namespace Server.Core.Repositories;

public interface IGuildLevelDefinitionRepository
{
    Task<GuildLevelDefinition?> GetByIdAsync(int id);
    Task AddAsync(GuildLevelDefinition entity);
    Task SaveAsync(GuildLevelDefinition entity);
    Task DeleteAsync(int id);
}