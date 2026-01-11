using Server.Core.Entities.Definitions;
using Server.Infrastructure.Contexts;
using Server.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Server.Infrastructure.Entities.Definitions;

namespace Server.Infrastructure.Repositories;

public class FarmPlotDefinitionRepository : IFarmPlotDefinitionRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public FarmPlotDefinitionRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<FarmPlotDefinition?> GetByIdAsync(Guid id)
    {
        var infraEntity = await _context.FarmPlotDefinitions.FindAsync(id);
        return _mapper.Map<FarmPlotDefinition>(infraEntity);
    }

    public async Task AddAsync(FarmPlotDefinition entity)
    {
        var infraEntity = _mapper.Map<FarmPlotDefinitionEntity>(entity);
        _context.FarmPlotDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(FarmPlotDefinition entity)
    {
        var infraEntity = _mapper.Map<FarmPlotDefinitionEntity>(entity);
        _context.FarmPlotDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var infraEntity = await _context.FarmPlotDefinitions.FindAsync(id);
        if (infraEntity != null)
        {
            _context.FarmPlotDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<FarmPlotDefinition?> GetByCodeAsync(string code)
    {
        var infraEntity = await _context.FarmPlotDefinitions.FirstOrDefaultAsync(e => e.Code == code);
        return _mapper.Map<FarmPlotDefinition>(infraEntity);
    }
    public async Task AddByCodeAsync(FarmPlotDefinition entity)
    {
        var infraEntity = _mapper.Map<FarmPlotDefinitionEntity>(entity);
        _context.FarmPlotDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task SaveByCodeAsync(FarmPlotDefinition entity)
    {
        var infraEntity = _mapper.Map<FarmPlotDefinitionEntity>(entity);
        _context.FarmPlotDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteByCodeAsync(string code)
    {
        var infraEntity = await _context.FarmPlotDefinitions.FirstOrDefaultAsync(e => e.Code == code);
        if (infraEntity != null)
        {
            _context.FarmPlotDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }
}