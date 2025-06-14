using MiniBoard.Core.Models;

namespace MiniBoard.Core.Repositories;

public interface IBoardTaskRepository
{
    Task<BoardTask?> GetByIdAsync(long id);
    Task<BoardTask?> GetByIdAndUserIdAsync(long id, long userId);
    Task<List<BoardTask>> GetByStatesAsync(List<TaskState> states);
    Task<List<BoardTask>> GetByStatesAndUserIdAsync(List<TaskState> states, long userId);
    Task<List<BoardTask>> GetByDueDateRangeAndUserIdAsync(DateTimeOffset startDate, DateTimeOffset endDate, long userId);
    Task<BoardTask> CreateAsync(BoardTask task);
    Task<BoardTask> UpdateAsync(BoardTask task);
    Task DeleteAsync(long id);
}