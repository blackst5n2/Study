using Server.Core.Entities.Entities;
using Server.Infrastructure.Contexts;
using Server.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Server.Infrastructure.Entities.Entities;

namespace Server.Infrastructure.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public AuthRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Auth?> GetByIdAsync(Guid id)
    {
        var infraEntity = await _context.Auths.FindAsync(id);
        return _mapper.Map<Auth>(infraEntity);
    }

    public async Task AddAsync(Auth entity)
    {
        var infraEntity = _mapper.Map<AuthEntity>(entity);
        _context.Auths.Add(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(Auth entity)
    {
        var infraEntity = _mapper.Map<AuthEntity>(entity);
        _context.Auths.Update(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var infraEntity = await _context.Auths.FindAsync(id);
        if (infraEntity != null)
        {
            _context.Auths.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }
}