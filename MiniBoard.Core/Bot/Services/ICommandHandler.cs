using Telegram.Bot.Types;

namespace MiniBoard.Core.Bot.Services;

public interface ICommandHandler
{
    Task<bool> HandleAsync(Message message, CancellationToken cancellationToken = default);
    ICommandHandler SetNext(ICommandHandler handler);
}