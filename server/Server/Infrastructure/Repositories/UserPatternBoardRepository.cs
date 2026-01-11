using Server.Core.Entities.UserContents;
using Server.Infrastructure.Contexts;
using Server.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Server.Infrastructure.Entities.UserContents;

namespace Server.Infrastructure.Repositories;

public class UserPatternBoardRepository : IUserPatternBoardRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public UserPatternBoardRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UserPatternBoard?> GetByIdAsync(Guid id)
    {
        var infraEntity = await _context.UserPatternBoards.FindAsync(id);
        return _mapper.Map<UserPatternBoard>(infraEntity);
    }

    public async Task AddAsync(UserPatternBoard entity)
    {
        var infraEntity = _mapper.Map<UserPatternBoardEntity>(entity);
        _context.UserPatternBoards.Add(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(UserPatternBoard entity)
    {
        var infraEntity = _mapper.Map<UserPatternBoardEntity>(entity);
        _context.UserPatternBoards.Update(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var infraEntity = await _context.UserPatternBoards.FindAsync(id);
        if (infraEntity != null)
        {
            _context.UserPatternBoards.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<UserPatternBoard?> GetByCodeAsync(string code)
    {
        var infraEntity = await _context.UserPatternBoards.FirstOrDefaultAsync(e => e.Code == code);
        return _mapper.Map<UserPatternBoard>(infraEntity);
    }
    public async Task AddByCodeAsync(UserPatternBoard entity)
    {
        var infraEntity = _mapper.Map<UserPatternBoardEntity>(entity);
        _context.UserPatternBoards.Add(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task SaveByCodeAsync(UserPatternBoard entity)
    {
        var infraEntity = _mapper.Map<UserPatternBoardEntity>(entity);
        _context.UserPatternBoards.Update(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteByCodeAsync(string code)
    {
        var infraEntity = await _context.UserPatternBoards.FirstOrDefaultAsync(e => e.Code == code);
        if (infraEntity != null)
        {
            _context.UserPatternBoards.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }
}