using Server.Core.Entities.Definitions;
using Server.Infrastructure.Contexts;
using Server.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Server.Infrastructure.Entities.Definitions;

namespace Server.Infrastructure.Repositories;

public class LivestockDefinitionRepository : ILivestockDefinitionRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public LivestockDefinitionRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<LivestockDefinition?> GetByIdAsync(Guid id)
    {
        var infraEntity = await _context.LivestockDefinitions.FindAsync(id);
        return _mapper.Map<LivestockDefinition>(infraEntity);
    }

    public async Task AddAsync(LivestockDefinition entity)
    {
        var infraEntity = _mapper.Map<LivestockDefinitionEntity>(entity);
        _context.LivestockDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(LivestockDefinition entity)
    {
        var infraEntity = _mapper.Map<LivestockDefinitionEntity>(entity);
        _context.LivestockDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var infraEntity = await _context.LivestockDefinitions.FindAsync(id);
        if (infraEntity != null)
        {
            _context.LivestockDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }
}