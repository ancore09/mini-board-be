namespace MiniBoard.Core.Models;

public class User
{
    public long Id { get; set; }
    public required string Email { get; set; }
    public required string Username { get; set; }
    public required string PasswordHash { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? LastLoginAt { get; set; }
    public bool IsActive { get; set; } = true;
    
    public string? TelegramUsername { get; set; }
    public long? TelegramChatId { get; set; }
}