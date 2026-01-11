using Server.Core.Entities.Definitions;
using Server.Infrastructure.Contexts;
using Server.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Server.Infrastructure.Entities.Definitions;

namespace Server.Infrastructure.Repositories;

public class JobDefinitionRepository : IJobDefinitionRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public JobDefinitionRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<JobDefinition?> GetByIdAsync(Guid id)
    {
        var infraEntity = await _context.JobDefinitions.FindAsync(id);
        return _mapper.Map<JobDefinition>(infraEntity);
    }

    public async Task AddAsync(JobDefinition entity)
    {
        var infraEntity = _mapper.Map<JobDefinitionEntity>(entity);
        _context.JobDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(JobDefinition entity)
    {
        var infraEntity = _mapper.Map<JobDefinitionEntity>(entity);
        _context.JobDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var infraEntity = await _context.JobDefinitions.FindAsync(id);
        if (infraEntity != null)
        {
            _context.JobDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<JobDefinition?> GetByCodeAsync(string code)
    {
        var infraEntity = await _context.JobDefinitions.FirstOrDefaultAsync(e => e.Code == code);
        return _mapper.Map<JobDefinition>(infraEntity);
    }
    public async Task AddByCodeAsync(JobDefinition entity)
    {
        var infraEntity = _mapper.Map<JobDefinitionEntity>(entity);
        _context.JobDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task SaveByCodeAsync(JobDefinition entity)
    {
        var infraEntity = _mapper.Map<JobDefinitionEntity>(entity);
        _context.JobDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteByCodeAsync(string code)
    {
        var infraEntity = await _context.JobDefinitions.FirstOrDefaultAsync(e => e.Code == code);
        if (infraEntity != null)
        {
            _context.JobDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }
}