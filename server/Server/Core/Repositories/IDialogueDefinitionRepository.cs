using Server.Core.Entities.Definitions;

namespace Server.Core.Repositories;

public interface IDialogueDefinitionRepository
{
    Task<DialogueDefinition?> GetByIdAsync(Guid id);
    Task AddAsync(DialogueDefinition entity);
    Task SaveAsync(DialogueDefinition entity);
    Task DeleteAsync(Guid id);
    Task<DialogueDefinition?> GetByCodeAsync(string code);
    Task AddByCodeAsync(DialogueDefinition entity);
    Task SaveByCodeAsync(DialogueDefinition entity);
    Task DeleteByCodeAsync(string code);
}