using Microsoft.EntityFrameworkCore;
using MiniBoard.Core.Models;
using MiniBoard.Core.Repositories;
using MiniBoard.Infra.Data;

namespace MiniBoard.Infra.Repositories;

public class UserRepository : IUserRepository
{
    private readonly MiniBoardDbContext _context;

    public UserRepository(MiniBoardDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(long id)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User?> GetByTelegramUsernameAsync(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.TelegramUsername == username);
    }

    public async Task<User?> GetByTelegramChatIdAsync(long chatId)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.TelegramChatId == chatId);
    }

    public async Task<User> CreateAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User> UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task DeleteAsync(long id)
    {
        var user = await GetByIdAsync(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}