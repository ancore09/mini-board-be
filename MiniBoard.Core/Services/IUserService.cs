using MiniBoard.Core.DTOs;
using MiniBoard.Core.Models;

namespace MiniBoard.Core.Services;

public interface IUserService
{
    Task<User?> GetByIdAsync(long id);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByTelegramUsernameAsync(string telegramUsername);
    Task<User?> GetByTelegramChatIdAsync(long chatId);
    Task<bool> UpdateTelegramUsernameAsync(long userId, UpdateUserTelegramUsernameDto dto);
}