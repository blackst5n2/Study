using Server.Core.Entities.Definitions;
using Server.Infrastructure.Contexts;
using Server.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Server.Infrastructure.Entities.Definitions;

namespace Server.Infrastructure.Repositories;

public class SeasonPassDefinitionRepository : ISeasonPassDefinitionRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public SeasonPassDefinitionRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<SeasonPassDefinition?> GetByIdAsync(Guid id)
    {
        var infraEntity = await _context.SeasonPassDefinitions.FindAsync(id);
        return _mapper.Map<SeasonPassDefinition>(infraEntity);
    }

    public async Task AddAsync(SeasonPassDefinition entity)
    {
        var infraEntity = _mapper.Map<SeasonPassDefinitionEntity>(entity);
        _context.SeasonPassDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(SeasonPassDefinition entity)
    {
        var infraEntity = _mapper.Map<SeasonPassDefinitionEntity>(entity);
        _context.SeasonPassDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var infraEntity = await _context.SeasonPassDefinitions.FindAsync(id);
        if (infraEntity != null)
        {
            _context.SeasonPassDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<SeasonPassDefinition?> GetByCodeAsync(string code)
    {
        var infraEntity = await _context.SeasonPassDefinitions.FirstOrDefaultAsync(e => e.Code == code);
        return _mapper.Map<SeasonPassDefinition>(infraEntity);
    }
    public async Task AddByCodeAsync(SeasonPassDefinition entity)
    {
        var infraEntity = _mapper.Map<SeasonPassDefinitionEntity>(entity);
        _context.SeasonPassDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task SaveByCodeAsync(SeasonPassDefinition entity)
    {
        var infraEntity = _mapper.Map<SeasonPassDefinitionEntity>(entity);
        _context.SeasonPassDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteByCodeAsync(string code)
    {
        var infraEntity = await _context.SeasonPassDefinitions.FirstOrDefaultAsync(e => e.Code == code);
        if (infraEntity != null)
        {
            _context.SeasonPassDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }
}