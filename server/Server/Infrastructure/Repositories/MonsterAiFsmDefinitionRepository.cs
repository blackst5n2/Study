using Server.Core.Entities.Definitions;
using Server.Infrastructure.Contexts;
using Server.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Server.Infrastructure.Entities.Definitions;

namespace Server.Infrastructure.Repositories;

public class MonsterAiFsmDefinitionRepository : IMonsterAiFsmDefinitionRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public MonsterAiFsmDefinitionRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<MonsterAiFsmDefinition?> GetByIdAsync(Guid id)
    {
        var infraEntity = await _context.MonsterAiFsmDefinitions.FindAsync(id);
        return _mapper.Map<MonsterAiFsmDefinition>(infraEntity);
    }

    public async Task AddAsync(MonsterAiFsmDefinition entity)
    {
        var infraEntity = _mapper.Map<MonsterAiFsmDefinitionEntity>(entity);
        _context.MonsterAiFsmDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(MonsterAiFsmDefinition entity)
    {
        var infraEntity = _mapper.Map<MonsterAiFsmDefinitionEntity>(entity);
        _context.MonsterAiFsmDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var infraEntity = await _context.MonsterAiFsmDefinitions.FindAsync(id);
        if (infraEntity != null)
        {
            _context.MonsterAiFsmDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<MonsterAiFsmDefinition?> GetByCodeAsync(string code)
    {
        var infraEntity = await _context.MonsterAiFsmDefinitions.FirstOrDefaultAsync(e => e.Code == code);
        return _mapper.Map<MonsterAiFsmDefinition>(infraEntity);
    }
    public async Task AddByCodeAsync(MonsterAiFsmDefinition entity)
    {
        var infraEntity = _mapper.Map<MonsterAiFsmDefinitionEntity>(entity);
        _context.MonsterAiFsmDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task SaveByCodeAsync(MonsterAiFsmDefinition entity)
    {
        var infraEntity = _mapper.Map<MonsterAiFsmDefinitionEntity>(entity);
        _context.MonsterAiFsmDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteByCodeAsync(string code)
    {
        var infraEntity = await _context.MonsterAiFsmDefinitions.FirstOrDefaultAsync(e => e.Code == code);
        if (infraEntity != null)
        {
            _context.MonsterAiFsmDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }
}