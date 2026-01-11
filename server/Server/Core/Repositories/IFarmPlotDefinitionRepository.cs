using Server.Core.Entities.Definitions;

namespace Server.Core.Repositories;

public interface IFarmPlotDefinitionRepository
{
    Task<FarmPlotDefinition?> GetByIdAsync(Guid id);
    Task AddAsync(FarmPlotDefinition entity);
    Task SaveAsync(FarmPlotDefinition entity);
    Task DeleteAsync(Guid id);
    Task<FarmPlotDefinition?> GetByCodeAsync(string code);
    Task AddByCodeAsync(FarmPlotDefinition entity);
    Task SaveByCodeAsync(FarmPlotDefinition entity);
    Task DeleteByCodeAsync(string code);
}