using Server.Core.Entities.Definitions;
using Server.Infrastructure.Contexts;
using Server.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Server.Infrastructure.Entities.Definitions;

namespace Server.Infrastructure.Repositories;

public class MonsterDefinitionRepository : IMonsterDefinitionRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public MonsterDefinitionRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<MonsterDefinition?> GetByIdAsync(Guid id)
    {
        var infraEntity = await _context.MonsterDefinitions.FindAsync(id);
        return _mapper.Map<MonsterDefinition>(infraEntity);
    }

    public async Task AddAsync(MonsterDefinition entity)
    {
        var infraEntity = _mapper.Map<MonsterDefinitionEntity>(entity);
        _context.MonsterDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(MonsterDefinition entity)
    {
        var infraEntity = _mapper.Map<MonsterDefinitionEntity>(entity);
        _context.MonsterDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var infraEntity = await _context.MonsterDefinitions.FindAsync(id);
        if (infraEntity != null)
        {
            _context.MonsterDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }
}