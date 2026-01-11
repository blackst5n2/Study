using Server.Core.Entities.UserContents;

namespace Server.Core.Repositories;

public interface IUserPatternBoardRepository
{
    Task<UserPatternBoard?> GetByIdAsync(Guid id);
    Task AddAsync(UserPatternBoard entity);
    Task SaveAsync(UserPatternBoard entity);
    Task DeleteAsync(Guid id);
    Task<UserPatternBoard?> GetByCodeAsync(string code);
    Task AddByCodeAsync(UserPatternBoard entity);
    Task SaveByCodeAsync(UserPatternBoard entity);
    Task DeleteByCodeAsync(string code);
}