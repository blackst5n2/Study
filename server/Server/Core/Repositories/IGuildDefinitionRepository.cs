using Server.Core.Entities.Definitions;

namespace Server.Core.Repositories;

public interface IGuildDefinitionRepository
{
    Task<GuildDefinition?> GetByIdAsync(Guid id);
    Task AddAsync(GuildDefinition entity);
    Task SaveAsync(GuildDefinition entity);
    Task DeleteAsync(Guid id);
}