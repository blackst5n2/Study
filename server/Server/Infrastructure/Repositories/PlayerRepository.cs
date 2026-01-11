using Server.Core.Entities.Progress;
using Server.Infrastructure.Contexts;
using Server.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Server.Infrastructure.Entities.Progress;

namespace Server.Infrastructure.Repositories;

public class PlayerRepository : IPlayerRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public PlayerRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Player?> GetByIdAsync(Guid id)
    {
        var infraEntity = await _context.Players.FindAsync(id);
        return _mapper.Map<Player>(infraEntity);
    }

    public async Task AddAsync(Player entity)
    {
        var infraEntity = _mapper.Map<PlayerEntity>(entity);
        _context.Players.Add(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(Player entity)
    {
        var infraEntity = _mapper.Map<PlayerEntity>(entity);
        _context.Players.Update(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var infraEntity = await _context.Players.FindAsync(id);
        if (infraEntity != null)
        {
            _context.Players.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Player?> GetByCodeAsync(string code)
    {
        var infraEntity = await _context.Players.FirstOrDefaultAsync(e => e.Code == code);
        return _mapper.Map<Player>(infraEntity);
    }
    public async Task AddByCodeAsync(Player entity)
    {
        var infraEntity = _mapper.Map<PlayerEntity>(entity);
        _context.Players.Add(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task SaveByCodeAsync(Player entity)
    {
        var infraEntity = _mapper.Map<PlayerEntity>(entity);
        _context.Players.Update(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteByCodeAsync(string code)
    {
        var infraEntity = await _context.Players.FirstOrDefaultAsync(e => e.Code == code);
        if (infraEntity != null)
        {
            _context.Players.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }
}