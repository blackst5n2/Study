using Server.Core.Entities.Definitions;
using Server.Infrastructure.Contexts;
using Server.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Server.Infrastructure.Entities.Definitions;

namespace Server.Infrastructure.Repositories;

public class MiniGameRewardDefinitionRepository : IMiniGameRewardDefinitionRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public MiniGameRewardDefinitionRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<MiniGameRewardDefinition?> GetByIdAsync(Guid id)
    {
        var infraEntity = await _context.MiniGameRewardDefinitions.FindAsync(id);
        return _mapper.Map<MiniGameRewardDefinition>(infraEntity);
    }

    public async Task AddAsync(MiniGameRewardDefinition entity)
    {
        var infraEntity = _mapper.Map<MiniGameRewardDefinitionEntity>(entity);
        _context.MiniGameRewardDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(MiniGameRewardDefinition entity)
    {
        var infraEntity = _mapper.Map<MiniGameRewardDefinitionEntity>(entity);
        _context.MiniGameRewardDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var infraEntity = await _context.MiniGameRewardDefinitions.FindAsync(id);
        if (infraEntity != null)
        {
            _context.MiniGameRewardDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }
}