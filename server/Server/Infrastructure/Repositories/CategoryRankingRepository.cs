using Server.Core.Entities.Entities;
using Server.Infrastructure.Contexts;
using Server.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Server.Infrastructure.Entities.Entities;

namespace Server.Infrastructure.Repositories;

public class CategoryRankingRepository : ICategoryRankingRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public CategoryRankingRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CategoryRanking?> GetByIdAsync(Guid id)
    {
        var infraEntity = await _context.CategoryRankings.FindAsync(id);
        return _mapper.Map<CategoryRanking>(infraEntity);
    }

    public async Task AddAsync(CategoryRanking entity)
    {
        var infraEntity = _mapper.Map<CategoryRankingEntity>(entity);
        _context.CategoryRankings.Add(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(CategoryRanking entity)
    {
        var infraEntity = _mapper.Map<CategoryRankingEntity>(entity);
        _context.CategoryRankings.Update(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var infraEntity = await _context.CategoryRankings.FindAsync(id);
        if (infraEntity != null)
        {
            _context.CategoryRankings.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }
}