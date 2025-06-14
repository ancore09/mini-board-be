using MiniBoard.Core.Models;

namespace MiniBoard.Core.Services;

public interface IBoardTaskService
{
    Task<List<BoardTask>> GetActiveTasksAsync();
    Task<List<BoardTask>> GetBacklogTasksAsync();
    Task<List<BoardTask>> GetTasksDueWithinWeekAsync();
    Task<BoardTask> CreateTaskAsync(CreateTaskDto dto);
    Task<BoardTask> UpdateTaskAsync(UpdateTaskDto dto);
    Task DeleteTaskAsync(long id);
}

public class CreateTaskDto
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public List<string> Tags { get; set; } = new();
    public TaskState State { get; set; }
    public Priority Priority { get; set; } = Priority.Medium;
    public DateTimeOffset? DueDate { get; set; }
}

public class UpdateTaskDto
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public List<string> Tags { get; set; } = new();
    public TaskState State { get; set; }
    public Priority Priority { get; set; } = Priority.Medium;
    public DateTimeOffset? DueDate { get; set; }
}

public class DeleteTaskDto
{
    public long Id { get; set; }
}