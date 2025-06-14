using MiniBoard.Core.Models;
using MiniBoard.Core.Repositories;

namespace MiniBoard.Core.Services;

public class BoardTaskService : IBoardTaskService
{
    private readonly IBoardTaskRepository _repository;
    private readonly IAuthContextService _authContext;

    public BoardTaskService(IBoardTaskRepository repository, IAuthContextService authContext)
    {
        _repository = repository;
        _authContext = authContext;
    }

    public async Task<List<BoardTask>> GetActiveTasksAsync()
    {
        if (!_authContext.IsAuthenticated || !_authContext.CurrentUserId.HasValue)
        {
            throw new UnauthorizedAccessException("User must be authenticated");
        }

        var activeStates = new List<TaskState> { TaskState.ToDo, TaskState.InProgress, TaskState.Review, TaskState.Done };
        return await _repository.GetByStatesAndUserIdAsync(activeStates, _authContext.CurrentUserId.Value);
    }

    public async Task<List<BoardTask>> GetBacklogTasksAsync()
    {
        if (!_authContext.IsAuthenticated || !_authContext.CurrentUserId.HasValue)
        {
            throw new UnauthorizedAccessException("User must be authenticated");
        }

        var backlogStates = new List<TaskState> { TaskState.Backlog };
        return await _repository.GetByStatesAndUserIdAsync(backlogStates, _authContext.CurrentUserId.Value);
    }

    public async Task<List<BoardTask>> GetTasksDueWithinWeekAsync()
    {
        if (!_authContext.IsAuthenticated || !_authContext.CurrentUserId.HasValue)
        {
            throw new UnauthorizedAccessException("User must be authenticated");
        }

        var now = DateTimeOffset.UtcNow;
        var oneWeekFromNow = now.AddDays(7);
        
        return await _repository.GetByDueDateRangeAndUserIdAsync(now, oneWeekFromNow, _authContext.CurrentUserId.Value);
    }

    public async Task<BoardTask> CreateTaskAsync(CreateTaskDto dto)
    {
        if (!_authContext.IsAuthenticated || !_authContext.CurrentUserId.HasValue)
        {
            throw new UnauthorizedAccessException("User must be authenticated");
        }

        var task = new BoardTask
        {
            Name = dto.Name,
            Description = dto.Description,
            Tags = dto.Tags,
            State = dto.State,
            Priority = dto.Priority,
            CreatedAt = DateTimeOffset.UtcNow,
            DueDate = dto.DueDate,
            UserId = _authContext.CurrentUserId.Value
        };

        return await _repository.CreateAsync(task);
    }

    public async Task<BoardTask> UpdateTaskAsync(UpdateTaskDto dto)
    {
        if (!_authContext.IsAuthenticated || !_authContext.CurrentUserId.HasValue)
        {
            throw new UnauthorizedAccessException("User must be authenticated");
        }

        var existingTask = await _repository.GetByIdAndUserIdAsync(dto.Id, _authContext.CurrentUserId.Value);
        if (existingTask == null)
        {
            throw new ArgumentException($"Task with id {dto.Id} not found or access denied");
        }

        existingTask.Name = dto.Name;
        existingTask.Description = dto.Description;
        existingTask.Tags = dto.Tags;
        existingTask.State = dto.State;
        existingTask.Priority = dto.Priority;
        existingTask.DueDate = dto.DueDate;

        return await _repository.UpdateAsync(existingTask);
    }

    public async Task DeleteTaskAsync(long id)
    {
        if (!_authContext.IsAuthenticated || !_authContext.CurrentUserId.HasValue)
        {
            throw new UnauthorizedAccessException("User must be authenticated");
        }

        var existingTask = await _repository.GetByIdAndUserIdAsync(id, _authContext.CurrentUserId.Value);
        if (existingTask == null)
        {
            throw new ArgumentException($"Task with id {id} not found or access denied");
        }

        await _repository.DeleteAsync(id);
    }
}