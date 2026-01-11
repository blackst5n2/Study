using Server.Core.Entities.Definitions;

namespace Server.Core.Repositories;

public interface IChannelDefinitionRepository
{
    Task<ChannelDefinition?> GetByIdAsync(Guid id);
    Task AddAsync(ChannelDefinition entity);
    Task SaveAsync(ChannelDefinition entity);
    Task DeleteAsync(Guid id);
    Task<ChannelDefinition?> GetByCodeAsync(string code);
    Task AddByCodeAsync(ChannelDefinition entity);
    Task SaveByCodeAsync(ChannelDefinition entity);
    Task DeleteByCodeAsync(string code);
}