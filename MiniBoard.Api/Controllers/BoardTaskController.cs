using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniBoard.Core.Models;
using MiniBoard.Core.Services;

namespace MiniBoard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BoardTaskController : ControllerBase
{
    private readonly IBoardTaskService _taskService;

    public BoardTaskController(IBoardTaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet("active")]
    public async Task<ActionResult<List<BoardTask>>> GetActiveTasks()
    {
        var tasks = await _taskService.GetActiveTasksAsync();
        return Ok(tasks);
    }

    [HttpGet("backlog")]
    public async Task<ActionResult<List<BoardTask>>> GetBacklogTasks()
    {
        var tasks = await _taskService.GetBacklogTasksAsync();
        return Ok(tasks);
    }

    [HttpPost("create")]
    public async Task<ActionResult<BoardTask>> CreateTask([FromBody] CreateTaskDto dto)
    {
        var task = await _taskService.CreateTaskAsync(dto);
        return Ok(task);
    }

    [HttpPost("update")]
    public async Task<ActionResult<BoardTask>> UpdateTask([FromBody] UpdateTaskDto dto)
    {
        try
        {
            var task = await _taskService.UpdateTaskAsync(dto);
            return Ok(task);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("delete")]
    public async Task<ActionResult> DeleteTask([FromBody] DeleteTaskDto dto)
    {
        await _taskService.DeleteTaskAsync(dto.Id);
        return Ok();
    }
}