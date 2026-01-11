using Server.Core.Entities.Definitions;
using Server.Infrastructure.Contexts;
using Server.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Server.Infrastructure.Entities.Definitions;

namespace Server.Infrastructure.Repositories;

public class LevelDefinitionRepository : ILevelDefinitionRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public LevelDefinitionRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<LevelDefinition?> GetByIdAsync(Guid id)
    {
        var infraEntity = await _context.LevelDefinitions.FindAsync(id);
        return _mapper.Map<LevelDefinition>(infraEntity);
    }

    public async Task AddAsync(LevelDefinition entity)
    {
        var infraEntity = _mapper.Map<LevelDefinitionEntity>(entity);
        _context.LevelDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(LevelDefinition entity)
    {
        var infraEntity = _mapper.Map<LevelDefinitionEntity>(entity);
        _context.LevelDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var infraEntity = await _context.LevelDefinitions.FindAsync(id);
        if (infraEntity != null)
        {
            _context.LevelDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<LevelDefinition?> GetByCodeAsync(string code)
    {
        var infraEntity = await _context.LevelDefinitions.FirstOrDefaultAsync(e => e.Code == code);
        return _mapper.Map<LevelDefinition>(infraEntity);
    }
    public async Task AddByCodeAsync(LevelDefinition entity)
    {
        var infraEntity = _mapper.Map<LevelDefinitionEntity>(entity);
        _context.LevelDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task SaveByCodeAsync(LevelDefinition entity)
    {
        var infraEntity = _mapper.Map<LevelDefinitionEntity>(entity);
        _context.LevelDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteByCodeAsync(string code)
    {
        var infraEntity = await _context.LevelDefinitions.FirstOrDefaultAsync(e => e.Code == code);
        if (infraEntity != null)
        {
            _context.LevelDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }
}