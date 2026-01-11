using Server.Core.Entities.Entities;
using Server.Infrastructure.Contexts;
using Server.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Server.Infrastructure.Entities.Entities;

namespace Server.Infrastructure.Repositories;

public class MailRepository : IMailRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public MailRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Mail?> GetByIdAsync(Guid id)
    {
        var infraEntity = await _context.Mails.FindAsync(id);
        return _mapper.Map<Mail>(infraEntity);
    }

    public async Task AddAsync(Mail entity)
    {
        var infraEntity = _mapper.Map<MailEntity>(entity);
        _context.Mails.Add(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(Mail entity)
    {
        var infraEntity = _mapper.Map<MailEntity>(entity);
        _context.Mails.Update(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var infraEntity = await _context.Mails.FindAsync(id);
        if (infraEntity != null)
        {
            _context.Mails.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Mail?> GetByCodeAsync(string code)
    {
        var infraEntity = await _context.Mails.FirstOrDefaultAsync(e => e.Code == code);
        return _mapper.Map<Mail>(infraEntity);
    }
    public async Task AddByCodeAsync(Mail entity)
    {
        var infraEntity = _mapper.Map<MailEntity>(entity);
        _context.Mails.Add(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task SaveByCodeAsync(Mail entity)
    {
        var infraEntity = _mapper.Map<MailEntity>(entity);
        _context.Mails.Update(infraEntity);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteByCodeAsync(string code)
    {
        var infraEntity = await _context.Mails.FirstOrDefaultAsync(e => e.Code == code);
        if (infraEntity != null)
        {
            _context.Mails.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }
}