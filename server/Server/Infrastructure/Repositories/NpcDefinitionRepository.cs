using Server.Core.Entities.Definitions;
using Server.Infrastructure.Contexts;
using Server.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Server.Infrastructure.Entities.Definitions;

namespace Server.Infrastructure.Repositories;

public class NpcDefinitionRepository : INpcDefinitionRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public NpcDefinitionRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<NpcDefinition?> GetByIdAsync(Guid id)
    {
        var infraEntity = await _context.NpcDefinitions.FindAsync(id);
        return _mapper.Map<NpcDefinition>(infraEntity);
    }

    public async Task AddAsync(NpcDefinition entity)
    {
        var infraEntity = _mapper.Map<NpcDefinitionEntity>(entity);
        _context.NpcDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(NpcDefinition entity)
    {
        var infraEntity = _mapper.Map<NpcDefinitionEntity>(entity);
        _context.NpcDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var infraEntity = await _context.NpcDefinitions.FindAsync(id);
        if (infraEntity != null)
        {
            _context.NpcDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<NpcDefinition?> GetByCodeAsync(string code)
    {
        var infraEntity = await _context.NpcDefinitions.FirstOrDefaultAsync(e => e.Code == code);
        return _mapper.Map<NpcDefinition>(infraEntity);
    }
    public async Task AddByCodeAsync(NpcDefinition entity)
    {
        var infraEntity = _mapper.Map<NpcDefinitionEntity>(entity);
        _context.NpcDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task SaveByCodeAsync(NpcDefinition entity)
    {
        var infraEntity = _mapper.Map<NpcDefinitionEntity>(entity);
        _context.NpcDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteByCodeAsync(string code)
    {
        var infraEntity = await _context.NpcDefinitions.FirstOrDefaultAsync(e => e.Code == code);
        if (infraEntity != null)
        {
            _context.NpcDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }
}