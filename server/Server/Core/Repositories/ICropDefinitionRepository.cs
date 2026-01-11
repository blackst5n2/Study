using Server.Core.Entities.Definitions;

namespace Server.Core.Repositories;

public interface ICropDefinitionRepository
{
    Task<CropDefinition?> GetByIdAsync(Guid id);
    Task AddAsync(CropDefinition entity);
    Task SaveAsync(CropDefinition entity);
    Task DeleteAsync(Guid id);
}