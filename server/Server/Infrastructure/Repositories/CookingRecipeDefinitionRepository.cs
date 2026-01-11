using Server.Core.Entities.Definitions;
using Server.Infrastructure.Contexts;
using Server.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Server.Infrastructure.Entities.Definitions;

namespace Server.Infrastructure.Repositories;

public class CookingRecipeDefinitionRepository : ICookingRecipeDefinitionRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public CookingRecipeDefinitionRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CookingRecipeDefinition?> GetByIdAsync(Guid id)
    {
        var infraEntity = await _context.CookingRecipeDefinitions.FindAsync(id);
        return _mapper.Map<CookingRecipeDefinition>(infraEntity);
    }

    public async Task AddAsync(CookingRecipeDefinition entity)
    {
        var infraEntity = _mapper.Map<CookingRecipeDefinitionEntity>(entity);
        _context.CookingRecipeDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(CookingRecipeDefinition entity)
    {
        var infraEntity = _mapper.Map<CookingRecipeDefinitionEntity>(entity);
        _context.CookingRecipeDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var infraEntity = await _context.CookingRecipeDefinitions.FindAsync(id);
        if (infraEntity != null)
        {
            _context.CookingRecipeDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<CookingRecipeDefinition?> GetByCodeAsync(string code)
    {
        var infraEntity = await _context.CookingRecipeDefinitions.FirstOrDefaultAsync(e => e.Code == code);
        return _mapper.Map<CookingRecipeDefinition>(infraEntity);
    }
    public async Task AddByCodeAsync(CookingRecipeDefinition entity)
    {
        var infraEntity = _mapper.Map<CookingRecipeDefinitionEntity>(entity);
        _context.CookingRecipeDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task SaveByCodeAsync(CookingRecipeDefinition entity)
    {
        var infraEntity = _mapper.Map<CookingRecipeDefinitionEntity>(entity);
        _context.CookingRecipeDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteByCodeAsync(string code)
    {
        var infraEntity = await _context.CookingRecipeDefinitions.FirstOrDefaultAsync(e => e.Code == code);
        if (infraEntity != null)
        {
            _context.CookingRecipeDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }
}