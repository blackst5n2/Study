using Server.Core.Entities.Definitions;
using Server.Infrastructure.Contexts;
using Server.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Server.Infrastructure.Entities.Definitions;

namespace Server.Infrastructure.Repositories;

public class RecipeDefinitionRepository : IRecipeDefinitionRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public RecipeDefinitionRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<RecipeDefinition?> GetByIdAsync(Guid id)
    {
        var infraEntity = await _context.RecipeDefinitions.FindAsync(id);
        return _mapper.Map<RecipeDefinition>(infraEntity);
    }

    public async Task AddAsync(RecipeDefinition entity)
    {
        var infraEntity = _mapper.Map<RecipeDefinitionEntity>(entity);
        _context.RecipeDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(RecipeDefinition entity)
    {
        var infraEntity = _mapper.Map<RecipeDefinitionEntity>(entity);
        _context.RecipeDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var infraEntity = await _context.RecipeDefinitions.FindAsync(id);
        if (infraEntity != null)
        {
            _context.RecipeDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<RecipeDefinition?> GetByCodeAsync(string code)
    {
        var infraEntity = await _context.RecipeDefinitions.FirstOrDefaultAsync(e => e.Code == code);
        return _mapper.Map<RecipeDefinition>(infraEntity);
    }
    public async Task AddByCodeAsync(RecipeDefinition entity)
    {
        var infraEntity = _mapper.Map<RecipeDefinitionEntity>(entity);
        _context.RecipeDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task SaveByCodeAsync(RecipeDefinition entity)
    {
        var infraEntity = _mapper.Map<RecipeDefinitionEntity>(entity);
        _context.RecipeDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteByCodeAsync(string code)
    {
        var infraEntity = await _context.RecipeDefinitions.FirstOrDefaultAsync(e => e.Code == code);
        if (infraEntity != null)
        {
            _context.RecipeDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }
}