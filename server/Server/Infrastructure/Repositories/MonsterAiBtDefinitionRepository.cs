using Server.Core.Entities.Definitions;
using Server.Infrastructure.Contexts;
using Server.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Server.Infrastructure.Entities.Definitions;

namespace Server.Infrastructure.Repositories;

public class MonsterAiBtDefinitionRepository : IMonsterAiBtDefinitionRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public MonsterAiBtDefinitionRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<MonsterAiBtDefinition?> GetByIdAsync(Guid id)
    {
        var infraEntity = await _context.MonsterAiBtDefinitions.FindAsync(id);
        return _mapper.Map<MonsterAiBtDefinition>(infraEntity);
    }

    public async Task AddAsync(MonsterAiBtDefinition entity)
    {
        var infraEntity = _mapper.Map<MonsterAiBtDefinitionEntity>(entity);
        _context.MonsterAiBtDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(MonsterAiBtDefinition entity)
    {
        var infraEntity = _mapper.Map<MonsterAiBtDefinitionEntity>(entity);
        _context.MonsterAiBtDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var infraEntity = await _context.MonsterAiBtDefinitions.FindAsync(id);
        if (infraEntity != null)
        {
            _context.MonsterAiBtDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<MonsterAiBtDefinition?> GetByCodeAsync(string code)
    {
        var infraEntity = await _context.MonsterAiBtDefinitions.FirstOrDefaultAsync(e => e.Code == code);
        return _mapper.Map<MonsterAiBtDefinition>(infraEntity);
    }
    public async Task AddByCodeAsync(MonsterAiBtDefinition entity)
    {
        var infraEntity = _mapper.Map<MonsterAiBtDefinitionEntity>(entity);
        _context.MonsterAiBtDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task SaveByCodeAsync(MonsterAiBtDefinition entity)
    {
        var infraEntity = _mapper.Map<MonsterAiBtDefinitionEntity>(entity);
        _context.MonsterAiBtDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteByCodeAsync(string code)
    {
        var infraEntity = await _context.MonsterAiBtDefinitions.FirstOrDefaultAsync(e => e.Code == code);
        if (infraEntity != null)
        {
            _context.MonsterAiBtDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }
}