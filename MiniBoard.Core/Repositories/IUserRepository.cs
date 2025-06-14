using MiniBoard.Core.Models;

namespace MiniBoard.Core.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(long id);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByTelegramUsernameAsync(string username);
    Task<User?> GetByTelegramChatIdAsync(long chatId);
    Task<User> CreateAsync(User user);
    Task<User> UpdateAsync(User user);
    Task DeleteAsync(long id);
}