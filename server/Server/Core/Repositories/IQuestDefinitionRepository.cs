using Server.Core.Entities.Definitions;

namespace Server.Core.Repositories;

public interface IQuestDefinitionRepository
{
    Task<QuestDefinition?> GetByIdAsync(Guid id);
    Task AddAsync(QuestDefinition entity);
    Task SaveAsync(QuestDefinition entity);
    Task DeleteAsync(Guid id);
    Task<QuestDefinition?> GetByCodeAsync(string code);
    Task AddByCodeAsync(QuestDefinition entity);
    Task SaveByCodeAsync(QuestDefinition entity);
    Task DeleteByCodeAsync(string code);
}