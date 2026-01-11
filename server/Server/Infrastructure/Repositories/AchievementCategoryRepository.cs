using Server.Core.Entities.Entities;
using Server.Infrastructure.Contexts;
using Server.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Server.Infrastructure.Entities.Entities;

namespace Server.Infrastructure.Repositories;

public class AchievementCategoryRepository : IAchievementCategoryRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public AchievementCategoryRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<AchievementCategory?> GetByIdAsync(Guid id)
    {
        var infraEntity = await _context.AchievementCategories.FindAsync(id);
        return _mapper.Map<AchievementCategory>(infraEntity);
    }

    public async Task AddAsync(AchievementCategory entity)
    {
        var infraEntity = _mapper.Map<AchievementCategoryEntity>(entity);
        _context.AchievementCategories.Add(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(AchievementCategory entity)
    {
        var infraEntity = _mapper.Map<AchievementCategoryEntity>(entity);
        _context.AchievementCategories.Update(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var infraEntity = await _context.AchievementCategories.FindAsync(id);
        if (infraEntity != null)
        {
            _context.AchievementCategories.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<AchievementCategory?> GetByCodeAsync(string code)
    {
        var infraEntity = await _context.AchievementCategories.FirstOrDefaultAsync(e => e.Code == code);
        return _mapper.Map<AchievementCategory>(infraEntity);
    }
    public async Task AddByCodeAsync(AchievementCategory entity)
    {
        var infraEntity = _mapper.Map<AchievementCategoryEntity>(entity);
        _context.AchievementCategories.Add(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task SaveByCodeAsync(AchievementCategory entity)
    {
        var infraEntity = _mapper.Map<AchievementCategoryEntity>(entity);
        _context.AchievementCategories.Update(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteByCodeAsync(string code)
    {
        var infraEntity = await _context.AchievementCategories.FirstOrDefaultAsync(e => e.Code == code);
        if (infraEntity != null)
        {
            _context.AchievementCategories.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }
}