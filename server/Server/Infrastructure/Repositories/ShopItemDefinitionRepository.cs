using Server.Core.Entities.Definitions;
using Server.Infrastructure.Contexts;
using Server.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Server.Infrastructure.Entities.Definitions;

namespace Server.Infrastructure.Repositories;

public class ShopItemDefinitionRepository : IShopItemDefinitionRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public ShopItemDefinitionRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ShopItemDefinition?> GetByIdAsync(Guid id)
    {
        var infraEntity = await _context.ShopItemDefinitions.FindAsync(id);
        return _mapper.Map<ShopItemDefinition>(infraEntity);
    }

    public async Task AddAsync(ShopItemDefinition entity)
    {
        var infraEntity = _mapper.Map<ShopItemDefinitionEntity>(entity);
        _context.ShopItemDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(ShopItemDefinition entity)
    {
        var infraEntity = _mapper.Map<ShopItemDefinitionEntity>(entity);
        _context.ShopItemDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var infraEntity = await _context.ShopItemDefinitions.FindAsync(id);
        if (infraEntity != null)
        {
            _context.ShopItemDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }
}