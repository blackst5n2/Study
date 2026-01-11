using Server.Core.Entities.Entities;
using Server.Infrastructure.Contexts;
using Server.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Server.Infrastructure.Entities.Entities;

namespace Server.Infrastructure.Repositories;

public class QuestGroupRepository : IQuestGroupRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public QuestGroupRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<QuestGroup?> GetByIdAsync(Guid id)
    {
        var infraEntity = await _context.QuestGroups.FindAsync(id);
        return _mapper.Map<QuestGroup>(infraEntity);
    }

    public async Task AddAsync(QuestGroup entity)
    {
        var infraEntity = _mapper.Map<QuestGroupEntity>(entity);
        _context.QuestGroups.Add(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(QuestGroup entity)
    {
        var infraEntity = _mapper.Map<QuestGroupEntity>(entity);
        _context.QuestGroups.Update(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var infraEntity = await _context.QuestGroups.FindAsync(id);
        if (infraEntity != null)
        {
            _context.QuestGroups.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<QuestGroup?> GetByCodeAsync(string code)
    {
        var infraEntity = await _context.QuestGroups.FirstOrDefaultAsync(e => e.Code == code);
        return _mapper.Map<QuestGroup>(infraEntity);
    }
    public async Task AddByCodeAsync(QuestGroup entity)
    {
        var infraEntity = _mapper.Map<QuestGroupEntity>(entity);
        _context.QuestGroups.Add(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task SaveByCodeAsync(QuestGroup entity)
    {
        var infraEntity = _mapper.Map<QuestGroupEntity>(entity);
        _context.QuestGroups.Update(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteByCodeAsync(string code)
    {
        var infraEntity = await _context.QuestGroups.FirstOrDefaultAsync(e => e.Code == code);
        if (infraEntity != null)
        {
            _context.QuestGroups.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }
}