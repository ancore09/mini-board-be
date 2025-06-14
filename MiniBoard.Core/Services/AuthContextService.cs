namespace MiniBoard.Core.Services;

public class AuthContextService : IAuthContextService
{
    public long? CurrentUserId { get; private set; }
    public string? CurrentUsername { get; private set; }
    public string? CurrentUserEmail { get; private set; }
    public bool IsAuthenticated => CurrentUserId.HasValue;

    public void SetCurrentUser(long userId, string username, string email)
    {
        CurrentUserId = userId;
        CurrentUsername = username;
        CurrentUserEmail = email;
    }

    public void Clear()
    {
        CurrentUserId = null;
        CurrentUsername = null;
        CurrentUserEmail = null;
    }
}