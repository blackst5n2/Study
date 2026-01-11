using Server.Core.Entities.Entities;

namespace Server.Core.Repositories;

public interface INotificationScheduleRepository
{
    Task<NotificationSchedule?> GetByIdAsync(Guid id);
    Task AddAsync(NotificationSchedule entity);
    Task SaveAsync(NotificationSchedule entity);
    Task DeleteAsync(Guid id);
}