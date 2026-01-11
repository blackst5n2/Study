using Server.Core.Entities.Entities;
using Server.Infrastructure.Contexts;
using Server.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Server.Infrastructure.Entities.Entities;

namespace Server.Infrastructure.Repositories;

public class NotificationScheduleRepository : INotificationScheduleRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public NotificationScheduleRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<NotificationSchedule?> GetByIdAsync(Guid id)
    {
        var infraEntity = await _context.NotificationSchedules.FindAsync(id);
        return _mapper.Map<NotificationSchedule>(infraEntity);
    }

    public async Task AddAsync(NotificationSchedule entity)
    {
        var infraEntity = _mapper.Map<NotificationScheduleEntity>(entity);
        _context.NotificationSchedules.Add(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(NotificationSchedule entity)
    {
        var infraEntity = _mapper.Map<NotificationScheduleEntity>(entity);
        _context.NotificationSchedules.Update(infraEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var infraEntity = await _context.NotificationSchedules.FindAsync(id);
        if (infraEntity != null)
        {
            _context.NotificationSchedules.Remove(infraEntity);
            await _context.SaveChangesAsync();
        }
    }
}