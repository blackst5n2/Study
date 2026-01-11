using Server.Core.Entities.Definitions;
using Server.Infrastructure.Contexts;
using Server.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Server.Infrastructure.Entities.Definitions;

namespace Server.Infrastructure.Repositories;

public class JobSkillLevelDefinitionRepository : IJobSkillLevelDefinitionRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public JobSkillLevelDefinitionRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<JobSkillLevelDefinition?> GetByIdAsync(Guid id)
    {
        var infraEntity = await _context.JobSkillLevelDefinitions.FindAsync(id);
        return _mapper.Map<JobSkillLevelDefinition>(infraEntity);
    }

    public async Task AddAsync(JobSkillLevelDefinition entity)
    {
        var infraEntity = _mapper.Map<JobSkillLevelDefinitionEntity>(entity);
        _context.JobSkillLevelDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(JobSkillLevelDefinition entity)
    {
        var infraEntity = _mapper.Map<JobSkillLevelDefinitionEntity>(entity);
        _context.JobSkillLevelDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var infraEntity = await _context.JobSkillLevelDefinitions.FindAsync(id);
        if (infraEntity != null)
        {
            _context.JobSkillLevelDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }
}