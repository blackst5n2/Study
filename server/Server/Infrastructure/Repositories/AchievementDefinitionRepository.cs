using Server.Core.Entities.Definitions;
using Server.Infrastructure.Contexts;
using Server.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Server.Infrastructure.Entities.Definitions;

namespace Server.Infrastructure.Repositories;

public class AchievementDefinitionRepository : IAchievementDefinitionRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public AchievementDefinitionRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<AchievementDefinition?> GetByIdAsync(Guid id)
    {
        var infraEntity = await _context.AchievementDefinitions.FindAsync(id);
        return _mapper.Map<AchievementDefinition>(infraEntity);
    }

    public async Task AddAsync(AchievementDefinition entity)
    {
        var infraEntity = _mapper.Map<AchievementDefinitionEntity>(entity);
        _context.AchievementDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(AchievementDefinition entity)
    {
        var infraEntity = _mapper.Map<AchievementDefinitionEntity>(entity);
        _context.AchievementDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var infraEntity = await _context.AchievementDefinitions.FindAsync(id);
        if (infraEntity != null)
        {
            _context.AchievementDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<AchievementDefinition?> GetByCodeAsync(string code)
    {
        var infraEntity = await _context.AchievementDefinitions.FirstOrDefaultAsync(e => e.Code == code);
        return _mapper.Map<AchievementDefinition>(infraEntity);
    }
    public async Task AddByCodeAsync(AchievementDefinition entity)
    {
        var infraEntity = _mapper.Map<AchievementDefinitionEntity>(entity);
        _context.AchievementDefinitions.Add(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task SaveByCodeAsync(AchievementDefinition entity)
    {
        var infraEntity = _mapper.Map<AchievementDefinitionEntity>(entity);
        _context.AchievementDefinitions.Update(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteByCodeAsync(string code)
    {
        var infraEntity = await _context.AchievementDefinitions.FirstOrDefaultAsync(e => e.Code == code);
        if (infraEntity != null)
        {
            _context.AchievementDefinitions.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }
}