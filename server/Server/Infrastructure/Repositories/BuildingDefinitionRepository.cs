using Server.Core.Entities.Definitions;
using Server.Infrastructure.Contexts;
using Server.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Server.Infrastructure.Entities.Definitions;

namespace Server.Infrastructure.Repositories;

public class BuildingDefinitionRepository : IBuildingDefinitionRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public BuildingDefinitionRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BuildingDefinition?> GetByIdAsync(Guid id)
    {
        var infraEntity = await _context.BuildingDefinitions.FindAsync(id);
        return _mapper.Map<BuildingDefinition>(infraEntity);
    }

    public async Task AddAsync(BuildingDefinition entity)
    {
        var infraEntity = _mapper.Map<BuildingDefinitionEntity>(entity);
        _context.BuildingDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(BuildingDefinition entity)
    {
        var infraEntity = _mapper.Map<BuildingDefinitionEntity>(entity);
        _context.BuildingDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var infraEntity = await _context.BuildingDefinitions.FindAsync(id);
        if (infraEntity != null)
        {
            _context.BuildingDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<BuildingDefinition?> GetByCodeAsync(string code)
    {
        var infraEntity = await _context.BuildingDefinitions.FirstOrDefaultAsync(e => e.Code == code);
        return _mapper.Map<BuildingDefinition>(infraEntity);
    }
    public async Task AddByCodeAsync(BuildingDefinition entity)
    {
        var infraEntity = _mapper.Map<BuildingDefinitionEntity>(entity);
        _context.BuildingDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task SaveByCodeAsync(BuildingDefinition entity)
    {
        var infraEntity = _mapper.Map<BuildingDefinitionEntity>(entity);
        _context.BuildingDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteByCodeAsync(string code)
    {
        var infraEntity = await _context.BuildingDefinitions.FirstOrDefaultAsync(e => e.Code == code);
        if (infraEntity != null)
        {
            _context.BuildingDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }
}