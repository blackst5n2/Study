using Server.Core.Repositories;
using Server.Core.Entities.Entities;
using Server.Core.Entities.Definitions;
using Server.Core.Entities.Details;
using Server.Core.Entities.Logs;
using Server.Core.Entities.Progress;
using Server.Core.Entities.Refs;
using Server.Core.Entities.UserContents;

namespace Server.Application.Services;

public class ClassSkillDefinitionService
{
    private readonly IClassSkillDefinitionRepository _repo;
    public ClassSkillDefinitionService(IClassSkillDefinitionRepository repo) => _repo = repo;

    public async Task<ClassSkillDefinition?> GetByIdAsync(Guid id) => await _repo.GetByIdAsync(id);
    public async Task AddAsync(ClassSkillDefinition entity) => await _repo.AddAsync(entity);
    public async Task SaveAsync(ClassSkillDefinition entity) => await _repo.SaveAsync(entity);
    public async Task DeleteAsync(Guid id) => await _repo.DeleteAsync(id);
    // TODO: 유스케이스별 메서드 추가
}
