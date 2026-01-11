using Server.Core.Entities.Definitions;
using Server.Infrastructure.Contexts;
using Server.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Server.Infrastructure.Entities.Definitions;

namespace Server.Infrastructure.Repositories;

public class BuildingUpgradeDefinitionRepository : IBuildingUpgradeDefinitionRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public BuildingUpgradeDefinitionRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BuildingUpgradeDefinition?> GetByIdAsync(Guid id)
    {
        var infraEntity = await _context.BuildingUpgradeDefinitions.FindAsync(id);
        return _mapper.Map<BuildingUpgradeDefinition>(infraEntity);
    }

    public async Task AddAsync(BuildingUpgradeDefinition entity)
    {
        var infraEntity = _mapper.Map<BuildingUpgradeDefinitionEntity>(entity);
        _context.BuildingUpgradeDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(BuildingUpgradeDefinition entity)
    {
        var infraEntity = _mapper.Map<BuildingUpgradeDefinitionEntity>(entity);
        _context.BuildingUpgradeDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var infraEntity = await _context.BuildingUpgradeDefinitions.FindAsync(id);
        if (infraEntity != null)
        {
            _context.BuildingUpgradeDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }
}