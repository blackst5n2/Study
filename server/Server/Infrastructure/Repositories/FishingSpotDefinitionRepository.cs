using Server.Core.Entities.Definitions;
using Server.Infrastructure.Contexts;
using Server.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Server.Infrastructure.Entities.Definitions;

namespace Server.Infrastructure.Repositories;

public class FishingSpotDefinitionRepository : IFishingSpotDefinitionRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public FishingSpotDefinitionRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<FishingSpotDefinition?> GetByIdAsync(Guid id)
    {
        var infraEntity = await _context.FishingSpotDefinitions.FindAsync(id);
        return _mapper.Map<FishingSpotDefinition>(infraEntity);
    }

    public async Task AddAsync(FishingSpotDefinition entity)
    {
        var infraEntity = _mapper.Map<FishingSpotDefinitionEntity>(entity);
        _context.FishingSpotDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(FishingSpotDefinition entity)
    {
        var infraEntity = _mapper.Map<FishingSpotDefinitionEntity>(entity);
        _context.FishingSpotDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var infraEntity = await _context.FishingSpotDefinitions.FindAsync(id);
        if (infraEntity != null)
        {
            _context.FishingSpotDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<FishingSpotDefinition?> GetByCodeAsync(string code)
    {
        var infraEntity = await _context.FishingSpotDefinitions.FirstOrDefaultAsync(e => e.Code == code);
        return _mapper.Map<FishingSpotDefinition>(infraEntity);
    }
    public async Task AddByCodeAsync(FishingSpotDefinition entity)
    {
        var infraEntity = _mapper.Map<FishingSpotDefinitionEntity>(entity);
        _context.FishingSpotDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task SaveByCodeAsync(FishingSpotDefinition entity)
    {
        var infraEntity = _mapper.Map<FishingSpotDefinitionEntity>(entity);
        _context.FishingSpotDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteByCodeAsync(string code)
    {
        var infraEntity = await _context.FishingSpotDefinitions.FirstOrDefaultAsync(e => e.Code == code);
        if (infraEntity != null)
        {
            _context.FishingSpotDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }
}