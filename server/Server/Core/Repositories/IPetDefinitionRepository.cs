using Server.Core.Entities.Definitions;

namespace Server.Core.Repositories;

public interface IPetDefinitionRepository
{
    Task<PetDefinition?> GetByIdAsync(Guid id);
    Task AddAsync(PetDefinition entity);
    Task SaveAsync(PetDefinition entity);
    Task DeleteAsync(Guid id);
    Task<PetDefinition?> GetByCodeAsync(string code);
    Task AddByCodeAsync(PetDefinition entity);
    Task SaveByCodeAsync(PetDefinition entity);
    Task DeleteByCodeAsync(string code);
}