using Server.Core.Entities.Definitions;
using Server.Infrastructure.Contexts;
using Server.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Server.Infrastructure.Entities.Definitions;

namespace Server.Infrastructure.Repositories;

public class CookingResultDefinitionRepository : ICookingResultDefinitionRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public CookingResultDefinitionRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CookingResultDefinition?> GetByIdAsync(Guid id)
    {
        var infraEntity = await _context.CookingResultDefinitions.FindAsync(id);
        return _mapper.Map<CookingResultDefinition>(infraEntity);
    }

    public async Task AddAsync(CookingResultDefinition entity)
    {
        var infraEntity = _mapper.Map<CookingResultDefinitionEntity>(entity);
        _context.CookingResultDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(CookingResultDefinition entity)
    {
        var infraEntity = _mapper.Map<CookingResultDefinitionEntity>(entity);
        _context.CookingResultDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var infraEntity = await _context.CookingResultDefinitions.FindAsync(id);
        if (infraEntity != null)
        {
            _context.CookingResultDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }
}