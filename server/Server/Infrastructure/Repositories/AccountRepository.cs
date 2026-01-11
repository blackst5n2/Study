using Server.Core.Entities.Entities;
using Server.Infrastructure.Contexts;
using Server.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Server.Infrastructure.Entities.Entities;

namespace Server.Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public AccountRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Account?> GetByIdAsync(Guid id)
    {
        var infraEntity = await _context.Accounts.FindAsync(id);
        return _mapper.Map<Account>(infraEntity);
    }

    public async Task AddAsync(Account entity)
    {
        var infraEntity = _mapper.Map<AccountEntity>(entity);
        _context.Accounts.Add(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(Account entity)
    {
        var infraEntity = _mapper.Map<AccountEntity>(entity);
        _context.Accounts.Update(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var infraEntity = await _context.Accounts.FindAsync(id);
        if (infraEntity != null)
        {
            _context.Accounts.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }
}