using Server.Core.Repositories;
using Server.Core.Entities.Entities;
using Server.Core.Entities.Definitions;
using Server.Core.Entities.Details;
using Server.Core.Entities.Logs;
using Server.Core.Entities.Progress;
using Server.Core.Entities.Refs;
using Server.Core.Entities.UserContents;

namespace Server.Application.Services;

public class SeasonPassDefinitionService
{
    private readonly ISeasonPassDefinitionRepository _repo;
    public SeasonPassDefinitionService(ISeasonPassDefinitionRepository repo) => _repo = repo;

    public async Task<SeasonPassDefinition?> GetByIdAsync(Guid id) => await _repo.GetByIdAsync(id);
    public async Task AddAsync(SeasonPassDefinition entity) => await _repo.AddAsync(entity);
    public async Task SaveAsync(SeasonPassDefinition entity) => await _repo.SaveAsync(entity);
    public async Task DeleteAsync(Guid id) => await _repo.DeleteAsync(id);
    public async Task<SeasonPassDefinition?> GetByCodeAsync(string code) => await _repo.GetByCodeAsync(code);
    public async Task AddByCodeAsync(SeasonPassDefinition entity) => await _repo.AddByCodeAsync(entity);
    public async Task SaveByCodeAsync(SeasonPassDefinition entity) => await _repo.SaveByCodeAsync(entity);
    public async Task DeleteByCodeAsync(string code) => await _repo.DeleteByCodeAsync(code);
    // TODO: 유스케이스별 메서드 추가
}
