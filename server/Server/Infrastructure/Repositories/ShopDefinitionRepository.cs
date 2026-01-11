using Server.Core.Entities.Definitions;
using Server.Infrastructure.Contexts;
using Server.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Server.Infrastructure.Entities.Definitions;

namespace Server.Infrastructure.Repositories;

public class ShopDefinitionRepository : IShopDefinitionRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public ShopDefinitionRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ShopDefinition?> GetByIdAsync(Guid id)
    {
        var infraEntity = await _context.ShopDefinitions.FindAsync(id);
        return _mapper.Map<ShopDefinition>(infraEntity);
    }

    public async Task AddAsync(ShopDefinition entity)
    {
        var infraEntity = _mapper.Map<ShopDefinitionEntity>(entity);
        _context.ShopDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(ShopDefinition entity)
    {
        var infraEntity = _mapper.Map<ShopDefinitionEntity>(entity);
        _context.ShopDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var infraEntity = await _context.ShopDefinitions.FindAsync(id);
        if (infraEntity != null)
        {
            _context.ShopDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<ShopDefinition?> GetByCodeAsync(string code)
    {
        var infraEntity = await _context.ShopDefinitions.FirstOrDefaultAsync(e => e.Code == code);
        return _mapper.Map<ShopDefinition>(infraEntity);
    }
    public async Task AddByCodeAsync(ShopDefinition entity)
    {
        var infraEntity = _mapper.Map<ShopDefinitionEntity>(entity);
        _context.ShopDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task SaveByCodeAsync(ShopDefinition entity)
    {
        var infraEntity = _mapper.Map<ShopDefinitionEntity>(entity);
        _context.ShopDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteByCodeAsync(string code)
    {
        var infraEntity = await _context.ShopDefinitions.FirstOrDefaultAsync(e => e.Code == code);
        if (infraEntity != null)
        {
            _context.ShopDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }
}