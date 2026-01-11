using Server.Core.Entities.Definitions;
using Server.Infrastructure.Contexts;
using Server.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Server.Infrastructure.Entities.Definitions;

namespace Server.Infrastructure.Repositories;

public class ItemDefinitionRepository : IItemDefinitionRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public ItemDefinitionRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ItemDefinition?> GetByIdAsync(Guid id)
    {
        var infraEntity = await _context.ItemDefinitions.FindAsync(id);
        return _mapper.Map<ItemDefinition>(infraEntity);
    }

    public async Task AddAsync(ItemDefinition entity)
    {
        var infraEntity = _mapper.Map<ItemDefinitionEntity>(entity);
        _context.ItemDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(ItemDefinition entity)
    {
        var infraEntity = _mapper.Map<ItemDefinitionEntity>(entity);
        _context.ItemDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var infraEntity = await _context.ItemDefinitions.FindAsync(id);
        if (infraEntity != null)
        {
            _context.ItemDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<ItemDefinition?> GetByCodeAsync(string code)
    {
        var infraEntity = await _context.ItemDefinitions.FirstOrDefaultAsync(e => e.Code == code);
        return _mapper.Map<ItemDefinition>(infraEntity);
    }
    public async Task AddByCodeAsync(ItemDefinition entity)
    {
        var infraEntity = _mapper.Map<ItemDefinitionEntity>(entity);
        _context.ItemDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task SaveByCodeAsync(ItemDefinition entity)
    {
        var infraEntity = _mapper.Map<ItemDefinitionEntity>(entity);
        _context.ItemDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteByCodeAsync(string code)
    {
        var infraEntity = await _context.ItemDefinitions.FirstOrDefaultAsync(e => e.Code == code);
        if (infraEntity != null)
        {
            _context.ItemDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }
}