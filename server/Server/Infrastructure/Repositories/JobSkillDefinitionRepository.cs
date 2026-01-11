using Server.Core.Entities.Definitions;
using Server.Infrastructure.Contexts;
using Server.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Server.Infrastructure.Entities.Definitions;

namespace Server.Infrastructure.Repositories;

public class JobSkillDefinitionRepository : IJobSkillDefinitionRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public JobSkillDefinitionRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<JobSkillDefinition?> GetByIdAsync(Guid id)
    {
        var infraEntity = await _context.JobSkillDefinitions.FindAsync(id);
        return _mapper.Map<JobSkillDefinition>(infraEntity);
    }

    public async Task AddAsync(JobSkillDefinition entity)
    {
        var infraEntity = _mapper.Map<JobSkillDefinitionEntity>(entity);
        _context.JobSkillDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(JobSkillDefinition entity)
    {
        var infraEntity = _mapper.Map<JobSkillDefinitionEntity>(entity);
        _context.JobSkillDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var infraEntity = await _context.JobSkillDefinitions.FindAsync(id);
        if (infraEntity != null)
        {
            _context.JobSkillDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<JobSkillDefinition?> GetByCodeAsync(string code)
    {
        var infraEntity = await _context.JobSkillDefinitions.FirstOrDefaultAsync(e => e.Code == code);
        return _mapper.Map<JobSkillDefinition>(infraEntity);
    }
    public async Task AddByCodeAsync(JobSkillDefinition entity)
    {
        var infraEntity = _mapper.Map<JobSkillDefinitionEntity>(entity);
        _context.JobSkillDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task SaveByCodeAsync(JobSkillDefinition entity)
    {
        var infraEntity = _mapper.Map<JobSkillDefinitionEntity>(entity);
        _context.JobSkillDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteByCodeAsync(string code)
    {
        var infraEntity = await _context.JobSkillDefinitions.FirstOrDefaultAsync(e => e.Code == code);
        if (infraEntity != null)
        {
            _context.JobSkillDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }
}