using Server.Core.Entities.Definitions;
using Server.Infrastructure.Contexts;
using Server.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Server.Infrastructure.Entities.Definitions;

namespace Server.Infrastructure.Repositories;

public class StatDefinitionRepository : IStatDefinitionRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public StatDefinitionRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<StatDefinition?> GetByIdAsync(Guid id)
    {
        var infraEntity = await _context.StatDefinitions.FindAsync(id);
        return _mapper.Map<StatDefinition>(infraEntity);
    }

    public async Task AddAsync(StatDefinition entity)
    {
        var infraEntity = _mapper.Map<StatDefinitionEntity>(entity);
        _context.StatDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(StatDefinition entity)
    {
        var infraEntity = _mapper.Map<StatDefinitionEntity>(entity);
        _context.StatDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var infraEntity = await _context.StatDefinitions.FindAsync(id);
        if (infraEntity != null)
        {
            _context.StatDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<StatDefinition?> GetByCodeAsync(string code)
    {
        var infraEntity = await _context.StatDefinitions.FirstOrDefaultAsync(e => e.Code == code);
        return _mapper.Map<StatDefinition>(infraEntity);
    }
    public async Task AddByCodeAsync(StatDefinition entity)
    {
        var infraEntity = _mapper.Map<StatDefinitionEntity>(entity);
        _context.StatDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task SaveByCodeAsync(StatDefinition entity)
    {
        var infraEntity = _mapper.Map<StatDefinitionEntity>(entity);
        _context.StatDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteByCodeAsync(string code)
    {
        var infraEntity = await _context.StatDefinitions.FirstOrDefaultAsync(e => e.Code == code);
        if (infraEntity != null)
        {
            _context.StatDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }
}