namespace MiniBoard.Core.Services;

public interface IAuthContextService
{
    long? CurrentUserId { get; }
    string? CurrentUsername { get; }
    string? CurrentUserEmail { get; }
    bool IsAuthenticated { get; }
    void SetCurrentUser(long userId, string username, string email);
    void Clear();
}