using Server.Core.Entities.Definitions;
using Server.Infrastructure.Contexts;
using Server.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Server.Infrastructure.Entities.Definitions;

namespace Server.Infrastructure.Repositories;

public class FishDefinitionRepository : IFishDefinitionRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public FishDefinitionRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<FishDefinition?> GetByIdAsync(Guid id)
    {
        var infraEntity = await _context.FishDefinitions.FindAsync(id);
        return _mapper.Map<FishDefinition>(infraEntity);
    }

    public async Task AddAsync(FishDefinition entity)
    {
        var infraEntity = _mapper.Map<FishDefinitionEntity>(entity);
        _context.FishDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(FishDefinition entity)
    {
        var infraEntity = _mapper.Map<FishDefinitionEntity>(entity);
        _context.FishDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var infraEntity = await _context.FishDefinitions.FindAsync(id);
        if (infraEntity != null)
        {
            _context.FishDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<FishDefinition?> GetByCodeAsync(string code)
    {
        var infraEntity = await _context.FishDefinitions.FirstOrDefaultAsync(e => e.Code == code);
        return _mapper.Map<FishDefinition>(infraEntity);
    }
    public async Task AddByCodeAsync(FishDefinition entity)
    {
        var infraEntity = _mapper.Map<FishDefinitionEntity>(entity);
        _context.FishDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task SaveByCodeAsync(FishDefinition entity)
    {
        var infraEntity = _mapper.Map<FishDefinitionEntity>(entity);
        _context.FishDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteByCodeAsync(string code)
    {
        var infraEntity = await _context.FishDefinitions.FirstOrDefaultAsync(e => e.Code == code);
        if (infraEntity != null)
        {
            _context.FishDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }
}