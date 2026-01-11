using Server.Core.Entities.Definitions;
using Server.Infrastructure.Contexts;
using Server.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Server.Infrastructure.Entities.Definitions;

namespace Server.Infrastructure.Repositories;

public class CookingStepDefinitionRepository : ICookingStepDefinitionRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public CookingStepDefinitionRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CookingStepDefinition?> GetByIdAsync(Guid id)
    {
        var infraEntity = await _context.CookingStepDefinitions.FindAsync(id);
        return _mapper.Map<CookingStepDefinition>(infraEntity);
    }

    public async Task AddAsync(CookingStepDefinition entity)
    {
        var infraEntity = _mapper.Map<CookingStepDefinitionEntity>(entity);
        _context.CookingStepDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(CookingStepDefinition entity)
    {
        var infraEntity = _mapper.Map<CookingStepDefinitionEntity>(entity);
        _context.CookingStepDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var infraEntity = await _context.CookingStepDefinitions.FindAsync(id);
        if (infraEntity != null)
        {
            _context.CookingStepDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }
}