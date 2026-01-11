using Server.Core.Entities.Entities;
using Server.Infrastructure.Contexts;
using Server.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Server.Infrastructure.Entities.Entities;

namespace Server.Infrastructure.Repositories;

public class TitleCategoryRepository : ITitleCategoryRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public TitleCategoryRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<TitleCategory?> GetByIdAsync(Guid id)
    {
        var infraEntity = await _context.TitleCategories.FindAsync(id);
        return _mapper.Map<TitleCategory>(infraEntity);
    }

    public async Task AddAsync(TitleCategory entity)
    {
        var infraEntity = _mapper.Map<TitleCategoryEntity>(entity);
        _context.TitleCategories.Add(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(TitleCategory entity)
    {
        var infraEntity = _mapper.Map<TitleCategoryEntity>(entity);
        _context.TitleCategories.Update(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var infraEntity = await _context.TitleCategories.FindAsync(id);
        if (infraEntity != null)
        {
            _context.TitleCategories.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<TitleCategory?> GetByCodeAsync(string code)
    {
        var infraEntity = await _context.TitleCategories.FirstOrDefaultAsync(e => e.Code == code);
        return _mapper.Map<TitleCategory>(infraEntity);
    }
    public async Task AddByCodeAsync(TitleCategory entity)
    {
        var infraEntity = _mapper.Map<TitleCategoryEntity>(entity);
        _context.TitleCategories.Add(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task SaveByCodeAsync(TitleCategory entity)
    {
        var infraEntity = _mapper.Map<TitleCategoryEntity>(entity);
        _context.TitleCategories.Update(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteByCodeAsync(string code)
    {
        var infraEntity = await _context.TitleCategories.FirstOrDefaultAsync(e => e.Code == code);
        if (infraEntity != null)
        {
            _context.TitleCategories.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }
}