using Server.Core.Entities.Definitions;
using Server.Infrastructure.Contexts;
using Server.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Server.Infrastructure.Entities.Definitions;

namespace Server.Infrastructure.Repositories;

public class ItemEffectDefinitionRepository : IItemEffectDefinitionRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public ItemEffectDefinitionRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ItemEffectDefinition?> GetByIdAsync(Guid id)
    {
        var infraEntity = await _context.ItemEffectDefinitions.FindAsync(id);
        return _mapper.Map<ItemEffectDefinition>(infraEntity);
    }

    public async Task AddAsync(ItemEffectDefinition entity)
    {
        var infraEntity = _mapper.Map<ItemEffectDefinitionEntity>(entity);
        _context.ItemEffectDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(ItemEffectDefinition entity)
    {
        var infraEntity = _mapper.Map<ItemEffectDefinitionEntity>(entity);
        _context.ItemEffectDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var infraEntity = await _context.ItemEffectDefinitions.FindAsync(id);
        if (infraEntity != null)
        {
            _context.ItemEffectDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<ItemEffectDefinition?> GetByCodeAsync(string code)
    {
        var infraEntity = await _context.ItemEffectDefinitions.FirstOrDefaultAsync(e => e.Code == code);
        return _mapper.Map<ItemEffectDefinition>(infraEntity);
    }
    public async Task AddByCodeAsync(ItemEffectDefinition entity)
    {
        var infraEntity = _mapper.Map<ItemEffectDefinitionEntity>(entity);
        _context.ItemEffectDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task SaveByCodeAsync(ItemEffectDefinition entity)
    {
        var infraEntity = _mapper.Map<ItemEffectDefinitionEntity>(entity);
        _context.ItemEffectDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteByCodeAsync(string code)
    {
        var infraEntity = await _context.ItemEffectDefinitions.FirstOrDefaultAsync(e => e.Code == code);
        if (infraEntity != null)
        {
            _context.ItemEffectDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }
}