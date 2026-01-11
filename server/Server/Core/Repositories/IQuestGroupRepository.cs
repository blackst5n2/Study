using Server.Core.Entities.Entities;

namespace Server.Core.Repositories;

public interface IQuestGroupRepository
{
    Task<QuestGroup?> GetByIdAsync(Guid id);
    Task AddAsync(QuestGroup entity);
    Task SaveAsync(QuestGroup entity);
    Task DeleteAsync(Guid id);
    Task<QuestGroup?> GetByCodeAsync(string code);
    Task AddByCodeAsync(QuestGroup entity);
    Task SaveByCodeAsync(QuestGroup entity);
    Task DeleteByCodeAsync(string code);
}