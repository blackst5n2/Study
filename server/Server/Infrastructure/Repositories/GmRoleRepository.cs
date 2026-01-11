using Server.Core.Entities.Entities;
using Server.Infrastructure.Contexts;
using Server.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Server.Infrastructure.Entities.Entities;

namespace Server.Infrastructure.Repositories;

public class GmRoleRepository : IGmRoleRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public GmRoleRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GmRole?> GetByIdAsync(Guid id)
    {
        var infraEntity = await _context.GmRoles.FindAsync(id);
        return _mapper.Map<GmRole>(infraEntity);
    }

    public async Task AddAsync(GmRole entity)
    {
        var infraEntity = _mapper.Map<GmRoleEntity>(entity);
        _context.GmRoles.Add(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(GmRole entity)
    {
        var infraEntity = _mapper.Map<GmRoleEntity>(entity);
        _context.GmRoles.Update(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var infraEntity = await _context.GmRoles.FindAsync(id);
        if (infraEntity != null)
        {
            _context.GmRoles.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<GmRole?> GetByCodeAsync(string code)
    {
        var infraEntity = await _context.GmRoles.FirstOrDefaultAsync(e => e.Code == code);
        return _mapper.Map<GmRole>(infraEntity);
    }
    public async Task AddByCodeAsync(GmRole entity)
    {
        var infraEntity = _mapper.Map<GmRoleEntity>(entity);
        _context.GmRoles.Add(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task SaveByCodeAsync(GmRole entity)
    {
        var infraEntity = _mapper.Map<GmRoleEntity>(entity);
        _context.GmRoles.Update(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteByCodeAsync(string code)
    {
        var infraEntity = await _context.GmRoles.FirstOrDefaultAsync(e => e.Code == code);
        if (infraEntity != null)
        {
            _context.GmRoles.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }
}