using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniBoard.Core.DTOs;
using MiniBoard.Core.Services;
using System.Security.Claims;

namespace MiniBoard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPut("telegram-username")]
    public async Task<IActionResult> UpdateTelegramUsername([FromBody] UpdateUserTelegramUsernameDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        try
        {
            var success = await _userService.UpdateTelegramUsernameAsync(userId, dto);
            if (!success)
            {
                return NotFound("User not found");
            }

            return Ok(new { message = "Telegram username updated successfully" });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!long.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var user = await _userService.GetByIdAsync(userId);
        if (user == null)
        {
            return NotFound();
        }

        return Ok(new
        {
            user.Id,
            user.Email,
            user.Username,
            user.TelegramUsername,
            user.CreatedAt,
            user.LastLoginAt,
            user.IsActive,
            user.TelegramChatId
        });
    }
}