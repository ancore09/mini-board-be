using MiniBoard.Core.DTOs;
using MiniBoard.Core.Models;
using MiniBoard.Core.Repositories;

namespace MiniBoard.Core.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User?> GetByIdAsync(long id)
    {
        return await _userRepository.GetByIdAsync(id);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _userRepository.GetByEmailAsync(email);
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _userRepository.GetByUsernameAsync(username);
    }

    public async Task<User?> GetByTelegramUsernameAsync(string telegramUsername)
    {
        return await _userRepository.GetByTelegramUsernameAsync(telegramUsername);
    }

    public async Task<User?> GetByTelegramChatIdAsync(long chatId)
    {
        return await _userRepository.GetByTelegramChatIdAsync(chatId);
    }

    public async Task<bool> UpdateTelegramUsernameAsync(long userId, UpdateUserTelegramUsernameDto dto)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            return false;
        }

        if (!string.IsNullOrWhiteSpace(dto.TelegramUsername) && 
            await _userRepository.GetByTelegramUsernameAsync(dto.TelegramUsername) != null)
        {
            throw new InvalidOperationException("Telegram username is already in use by another user.");
        }

        user.TelegramUsername = string.IsNullOrWhiteSpace(dto.TelegramUsername) ? null : dto.TelegramUsername;
        user.TelegramChatId = null;
        await _userRepository.UpdateAsync(user);
        return true;
    }
}