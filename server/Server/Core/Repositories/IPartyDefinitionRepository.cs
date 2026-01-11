using Server.Core.Entities.Definitions;

namespace Server.Core.Repositories;

public interface IPartyDefinitionRepository
{
    Task<PartyDefinition?> GetByIdAsync(Guid id);
    Task AddAsync(PartyDefinition entity);
    Task SaveAsync(PartyDefinition entity);
    Task DeleteAsync(Guid id);
}