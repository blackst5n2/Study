using Server.Core.Entities.Definitions;
using Server.Infrastructure.Contexts;
using Server.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Server.Infrastructure.Entities.Definitions;

namespace Server.Infrastructure.Repositories;

public class JobLevelDefinitionRepository : IJobLevelDefinitionRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public JobLevelDefinitionRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<JobLevelDefinition?> GetByIdAsync(Guid id)
    {
        var infraEntity = await _context.JobLevelDefinitions.FindAsync(id);
        return _mapper.Map<JobLevelDefinition>(infraEntity);
    }

    public async Task AddAsync(JobLevelDefinition entity)
    {
        var infraEntity = _mapper.Map<JobLevelDefinitionEntity>(entity);
        _context.JobLevelDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(JobLevelDefinition entity)
    {
        var infraEntity = _mapper.Map<JobLevelDefinitionEntity>(entity);
        _context.JobLevelDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var infraEntity = await _context.JobLevelDefinitions.FindAsync(id);
        if (infraEntity != null)
        {
            _context.JobLevelDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }
}