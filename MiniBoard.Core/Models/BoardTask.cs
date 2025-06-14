namespace MiniBoard.Core.Models;

public class BoardTask
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public List<string>? Tags { get; set; }
    public TaskState State { get; set; }
    public Priority Priority { get; set; } = Priority.Medium;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? DueDate { get; set; }
    public long UserId { get; set; }
    public User? User { get; set; }
}

public enum TaskState
{
    Backlog,
    ToDo,
    InProgress,
    Review,
    Done,
    Cancelled,
}

public enum Priority
{
    Low = 0,
    Medium = 1,
    High = 2
}