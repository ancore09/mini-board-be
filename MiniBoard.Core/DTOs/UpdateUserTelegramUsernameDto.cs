using System.ComponentModel.DataAnnotations;

namespace MiniBoard.Core.DTOs;

public class UpdateUserTelegramUsernameDto
{
    [StringLength(32, ErrorMessage = "Telegram username cannot exceed 32 characters")]
    public string? TelegramUsername { get; set; }
}