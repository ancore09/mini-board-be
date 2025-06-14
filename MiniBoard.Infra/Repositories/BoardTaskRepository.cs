using Microsoft.EntityFrameworkCore;
using MiniBoard.Core.Models;
using MiniBoard.Core.Repositories;
using MiniBoard.Infra.Data;

namespace MiniBoard.Infra.Repositories;

public class BoardTaskRepository : IBoardTaskRepository
{
    private readonly MiniBoardDbContext _context;

    public BoardTaskRepository(MiniBoardDbContext context)
    {
        _context = context;
    }

    public async Task<BoardTask?> GetByIdAsync(long id)
    {
        return await _context.BoardTasks
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<BoardTask?> GetByIdAndUserIdAsync(long id, long userId)
    {
        return await _context.BoardTasks
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
    }

    public async Task<List<BoardTask>> GetByStatesAsync(List<TaskState> states)
    {
        return await _context.BoardTasks
            .Where(t => states.Contains(t.State))
            .OrderBy(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<BoardTask>> GetByStatesAndUserIdAsync(List<TaskState> states, long userId)
    {
        return await _context.BoardTasks
            .Where(t => states.Contains(t.State) && t.UserId == userId)
            .OrderBy(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<BoardTask>> GetByDueDateRangeAndUserIdAsync(DateTimeOffset startDate, DateTimeOffset endDate, long userId)
    {
        return await _context.BoardTasks
            .Where(t => t.UserId == userId && t.DueDate.HasValue && 
                       t.DueDate.Value >= startDate && t.DueDate.Value <= endDate)
            .OrderBy(t => t.DueDate)
            .ToListAsync();
    }

    public async Task<BoardTask> CreateAsync(BoardTask task)
    {
        _context.BoardTasks.Add(task);
        await _context.SaveChangesAsync();
        return task;
    }

    public async Task<BoardTask> UpdateAsync(BoardTask task)
    {
        _context.BoardTasks.Update(task);
        await _context.SaveChangesAsync();
        return task;
    }

    public async Task DeleteAsync(long id)
    {
        var task = await _context.BoardTasks.FindAsync(id);
        if (task != null)
        {
            _context.BoardTasks.Remove(task);
            await _context.SaveChangesAsync();
        }
    }
}