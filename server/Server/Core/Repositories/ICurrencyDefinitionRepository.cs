using Server.Core.Entities.Definitions;

namespace Server.Core.Repositories;

public interface ICurrencyDefinitionRepository
{
    Task<CurrencyDefinition?> GetByIdAsync(Guid id);
    Task AddAsync(CurrencyDefinition entity);
    Task SaveAsync(CurrencyDefinition entity);
    Task DeleteAsync(Guid id);
    Task<CurrencyDefinition?> GetByCodeAsync(string code);
    Task AddByCodeAsync(CurrencyDefinition entity);
    Task SaveByCodeAsync(CurrencyDefinition entity);
    Task DeleteByCodeAsync(string code);
}