using Server.Core.Entities.Entities;
using Server.Infrastructure.Contexts;
using Server.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Server.Infrastructure.Entities.Entities;

namespace Server.Infrastructure.Repositories;

public class TagRepository : ITagRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public TagRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Tag?> GetByIdAsync(Guid id)
    {
        var infraEntity = await _context.Tags.FindAsync(id);
        return _mapper.Map<Tag>(infraEntity);
    }

    public async Task AddAsync(Tag entity)
    {
        var infraEntity = _mapper.Map<TagEntity>(entity);
        _context.Tags.Add(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(Tag entity)
    {
        var infraEntity = _mapper.Map<TagEntity>(entity);
        _context.Tags.Update(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var infraEntity = await _context.Tags.FindAsync(id);
        if (infraEntity != null)
        {
            _context.Tags.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Tag?> GetByCodeAsync(string code)
    {
        var infraEntity = await _context.Tags.FirstOrDefaultAsync(e => e.Code == code);
        return _mapper.Map<Tag>(infraEntity);
    }
    public async Task AddByCodeAsync(Tag entity)
    {
        var infraEntity = _mapper.Map<TagEntity>(entity);
        _context.Tags.Add(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task SaveByCodeAsync(Tag entity)
    {
        var infraEntity = _mapper.Map<TagEntity>(entity);
        _context.Tags.Update(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteByCodeAsync(string code)
    {
        var infraEntity = await _context.Tags.FirstOrDefaultAsync(e => e.Code == code);
        if (infraEntity != null)
        {
            _context.Tags.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }
}