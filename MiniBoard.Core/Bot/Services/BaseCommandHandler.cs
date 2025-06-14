using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace MiniBoard.Core.Bot.Services;

public abstract class BaseCommandHandler : ICommandHandler
{
    private ICommandHandler? _nextHandler;
    protected readonly ITelegramBotClient Bot;
    protected readonly ILogger Logger;

    protected BaseCommandHandler(ITelegramBotClient bot, ILogger logger)
    {
        Bot = bot;
        Logger = logger;
    }

    public ICommandHandler SetNext(ICommandHandler handler)
    {
        _nextHandler = handler;
        return handler;
    }

    public virtual async Task<bool> HandleAsync(Message message, CancellationToken cancellationToken = default)
    {
        if (await CanHandle(message))
        {
            await ProcessCommand(message, cancellationToken);
            return true;
        }

        return _nextHandler?.HandleAsync(message, cancellationToken).Result ?? false;
    }

    protected abstract Task<bool> CanHandle(Message message);
    protected abstract Task ProcessCommand(Message message, CancellationToken cancellationToken);
}