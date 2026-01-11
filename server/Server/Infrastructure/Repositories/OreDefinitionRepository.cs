using Server.Core.Entities.Definitions;
using Server.Infrastructure.Contexts;
using Server.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Server.Infrastructure.Entities.Definitions;

namespace Server.Infrastructure.Repositories;

public class OreDefinitionRepository : IOreDefinitionRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public OreDefinitionRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<OreDefinition?> GetByIdAsync(Guid id)
    {
        var infraEntity = await _context.OreDefinitions.FindAsync(id);
        return _mapper.Map<OreDefinition>(infraEntity);
    }

    public async Task AddAsync(OreDefinition entity)
    {
        var infraEntity = _mapper.Map<OreDefinitionEntity>(entity);
        _context.OreDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(OreDefinition entity)
    {
        var infraEntity = _mapper.Map<OreDefinitionEntity>(entity);
        _context.OreDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var infraEntity = await _context.OreDefinitions.FindAsync(id);
        if (infraEntity != null)
        {
            _context.OreDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }
}