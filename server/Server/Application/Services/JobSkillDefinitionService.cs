using Server.Core.Repositories;
using Server.Core.Entities.Entities;
using Server.Core.Entities.Definitions;
using Server.Core.Entities.Details;
using Server.Core.Entities.Logs;
using Server.Core.Entities.Progress;
using Server.Core.Entities.Refs;
using Server.Core.Entities.UserContents;

namespace Server.Application.Services;

public class JobSkillDefinitionService
{
    private readonly IJobSkillDefinitionRepository _repo;
    public JobSkillDefinitionService(IJobSkillDefinitionRepository repo) => _repo = repo;

    public async Task<JobSkillDefinition?> GetByIdAsync(Guid id) => await _repo.GetByIdAsync(id);
    public async Task AddAsync(JobSkillDefinition entity) => await _repo.AddAsync(entity);
    public async Task SaveAsync(JobSkillDefinition entity) => await _repo.SaveAsync(entity);
    public async Task DeleteAsync(Guid id) => await _repo.DeleteAsync(id);
    public async Task<JobSkillDefinition?> GetByCodeAsync(string code) => await _repo.GetByCodeAsync(code);
    public async Task AddByCodeAsync(JobSkillDefinition entity) => await _repo.AddByCodeAsync(entity);
    public async Task SaveByCodeAsync(JobSkillDefinition entity) => await _repo.SaveByCodeAsync(entity);
    public async Task DeleteByCodeAsync(string code) => await _repo.DeleteByCodeAsync(code);
    // TODO: 유스케이스별 메서드 추가
}
