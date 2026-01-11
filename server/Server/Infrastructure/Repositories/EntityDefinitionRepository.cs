using Server.Core.Entities.Definitions;
using Server.Infrastructure.Contexts;
using Server.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Server.Infrastructure.Entities.Definitions;

namespace Server.Infrastructure.Repositories;

public class EntityDefinitionRepository : IEntityDefinitionRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public EntityDefinitionRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<EntityDefinition?> GetByIdAsync(Guid id)
    {
        var infraEntity = await _context.EntityDefinitions.FindAsync(id);
        return _mapper.Map<EntityDefinition>(infraEntity);
    }

    public async Task AddAsync(EntityDefinition entity)
    {
        var infraEntity = _mapper.Map<EntityDefinitionEntity>(entity);
        _context.EntityDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(EntityDefinition entity)
    {
        var infraEntity = _mapper.Map<EntityDefinitionEntity>(entity);
        _context.EntityDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var infraEntity = await _context.EntityDefinitions.FindAsync(id);
        if (infraEntity != null)
        {
            _context.EntityDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<EntityDefinition?> GetByCodeAsync(string code)
    {
        var infraEntity = await _context.EntityDefinitions.FirstOrDefaultAsync(e => e.Code == code);
        return _mapper.Map<EntityDefinition>(infraEntity);
    }
    public async Task AddByCodeAsync(EntityDefinition entity)
    {
        var infraEntity = _mapper.Map<EntityDefinitionEntity>(entity);
        _context.EntityDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task SaveByCodeAsync(EntityDefinition entity)
    {
        var infraEntity = _mapper.Map<EntityDefinitionEntity>(entity);
        _context.EntityDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteByCodeAsync(string code)
    {
        var infraEntity = await _context.EntityDefinitions.FirstOrDefaultAsync(e => e.Code == code);
        if (infraEntity != null)
        {
            _context.EntityDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }
}