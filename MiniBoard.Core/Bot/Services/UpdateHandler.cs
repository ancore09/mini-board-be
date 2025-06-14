using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace MiniBoard.Core.Bot.Services;

public class UpdateHandler : IUpdateHandler
{
    private readonly ILogger<UpdateHandler> _logger;
    private readonly ICommandHandler _commandHandlerChain;

    public UpdateHandler(ILogger<UpdateHandler> logger, ICommandHandler commandHandlerChain)
    {
        _logger = logger;
        _commandHandlerChain = commandHandlerChain;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await (update switch
        {
            { Message: { } message } => OnMessage(message, cancellationToken),
            _ => UnknownUpdateHandlerAsync(update)
        });
    }

    public async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("HandleError: {Exception}", exception);
        // Cooldown in case of network connection error
        if (exception is RequestException)
            await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
    }
    
    private async Task OnMessage(Message message, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Receive message {msg} from {chat} (user = {user})", 
            message.Text, message.Chat.Id, message.Chat.Username);
        
        if (message.Text is null)
            return;
        
        await _commandHandlerChain.HandleAsync(message, cancellationToken);
    }
    
    private Task UnknownUpdateHandlerAsync(Update update)
    {
        _logger.LogInformation("Unknown update type: {UpdateType}", update.Type);
        return Task.CompletedTask;
    }
}