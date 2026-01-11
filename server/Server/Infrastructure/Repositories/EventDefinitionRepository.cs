using Server.Core.Entities.Definitions;
using Server.Infrastructure.Contexts;
using Server.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Server.Infrastructure.Entities.Definitions;

namespace Server.Infrastructure.Repositories;

public class EventDefinitionRepository : IEventDefinitionRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public EventDefinitionRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<EventDefinition?> GetByIdAsync(Guid id)
    {
        var infraEntity = await _context.EventDefinitions.FindAsync(id);
        return _mapper.Map<EventDefinition>(infraEntity);
    }

    public async Task AddAsync(EventDefinition entity)
    {
        var infraEntity = _mapper.Map<EventDefinitionEntity>(entity);
        _context.EventDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(EventDefinition entity)
    {
        var infraEntity = _mapper.Map<EventDefinitionEntity>(entity);
        _context.EventDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var infraEntity = await _context.EventDefinitions.FindAsync(id);
        if (infraEntity != null)
        {
            _context.EventDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<EventDefinition?> GetByCodeAsync(string code)
    {
        var infraEntity = await _context.EventDefinitions.FirstOrDefaultAsync(e => e.Code == code);
        return _mapper.Map<EventDefinition>(infraEntity);
    }
    public async Task AddByCodeAsync(EventDefinition entity)
    {
        var infraEntity = _mapper.Map<EventDefinitionEntity>(entity);
        _context.EventDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task SaveByCodeAsync(EventDefinition entity)
    {
        var infraEntity = _mapper.Map<EventDefinitionEntity>(entity);
        _context.EventDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteByCodeAsync(string code)
    {
        var infraEntity = await _context.EventDefinitions.FirstOrDefaultAsync(e => e.Code == code);
        if (infraEntity != null)
        {
            _context.EventDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }
}