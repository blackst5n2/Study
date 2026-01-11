using Server.Core.Repositories;
using Server.Core.Entities.Entities;
using Server.Core.Entities.Definitions;
using Server.Core.Entities.Details;
using Server.Core.Entities.Logs;
using Server.Core.Entities.Progress;
using Server.Core.Entities.Refs;
using Server.Core.Entities.UserContents;

namespace Server.Application.Services;

public class ChannelDefinitionService
{
    private readonly IChannelDefinitionRepository _repo;
    public ChannelDefinitionService(IChannelDefinitionRepository repo) => _repo = repo;

    public async Task<ChannelDefinition?> GetByIdAsync(Guid id) => await _repo.GetByIdAsync(id);
    public async Task AddAsync(ChannelDefinition entity) => await _repo.AddAsync(entity);
    public async Task SaveAsync(ChannelDefinition entity) => await _repo.SaveAsync(entity);
    public async Task DeleteAsync(Guid id) => await _repo.DeleteAsync(id);
    public async Task<ChannelDefinition?> GetByCodeAsync(string code) => await _repo.GetByCodeAsync(code);
    public async Task AddByCodeAsync(ChannelDefinition entity) => await _repo.AddByCodeAsync(entity);
    public async Task SaveByCodeAsync(ChannelDefinition entity) => await _repo.SaveByCodeAsync(entity);
    public async Task DeleteByCodeAsync(string code) => await _repo.DeleteByCodeAsync(code);
    // TODO: 유스케이스별 메서드 추가
}
