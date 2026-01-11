using Server.Core.Entities.Definitions;
using Server.Infrastructure.Contexts;
using Server.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Server.Infrastructure.Entities.Definitions;

namespace Server.Infrastructure.Repositories;

public class LogDefinitionRepository : ILogDefinitionRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public LogDefinitionRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<LogDefinition?> GetByIdAsync(Guid id)
    {
        var infraEntity = await _context.LogDefinitions.FindAsync(id);
        return _mapper.Map<LogDefinition>(infraEntity);
    }

    public async Task AddAsync(LogDefinition entity)
    {
        var infraEntity = _mapper.Map<LogDefinitionEntity>(entity);
        _context.LogDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(LogDefinition entity)
    {
        var infraEntity = _mapper.Map<LogDefinitionEntity>(entity);
        _context.LogDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var infraEntity = await _context.LogDefinitions.FindAsync(id);
        if (infraEntity != null)
        {
            _context.LogDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }
}