using Server.Core.Repositories;
using Server.Core.Entities.Entities;
using Server.Core.Entities.Definitions;
using Server.Core.Entities.Details;
using Server.Core.Entities.Logs;
using Server.Core.Entities.Progress;
using Server.Core.Entities.Refs;
using Server.Core.Entities.UserContents;

namespace Server.Application.Services;

public class LogDefinitionService
{
    private readonly ILogDefinitionRepository _repo;
    public LogDefinitionService(ILogDefinitionRepository repo) => _repo = repo;

    public async Task<LogDefinition?> GetByIdAsync(Guid id) => await _repo.GetByIdAsync(id);
    public async Task AddAsync(LogDefinition entity) => await _repo.AddAsync(entity);
    public async Task SaveAsync(LogDefinition entity) => await _repo.SaveAsync(entity);
    public async Task DeleteAsync(Guid id) => await _repo.DeleteAsync(id);
    // TODO: 유스케이스별 메서드 추가
}
