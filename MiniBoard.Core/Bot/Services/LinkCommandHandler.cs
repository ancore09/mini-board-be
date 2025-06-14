using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MiniBoard.Core.Repositories;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace MiniBoard.Core.Bot.Services;

public class LinkCommandHandler : BaseCommandHandler
{
    private readonly IServiceProvider _serviceProvider;

    public LinkCommandHandler(ITelegramBotClient bot, ILogger<LinkCommandHandler> logger, IServiceProvider serviceProvider) 
        : base(bot, logger)
    {
        _serviceProvider = serviceProvider;
    }

    protected override Task<bool> CanHandle(Message message)
    {
        return Task.FromResult(message.Text?.Split(' ')[0] == "/link");
    }

    protected override async Task ProcessCommand(Message message, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Processing /link command for user {Username} in chat {ChatId}", 
            message.Chat.Username, message.Chat.Id);

        var username = message.Chat.Username;
        
        if (string.IsNullOrWhiteSpace(username))
        {
            await Bot.SendMessage(message.Chat, "Error linking user: no username provided", cancellationToken: cancellationToken);
            return;
        }

        using var scope = _serviceProvider.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        
        var user = await repository.GetByTelegramUsernameAsync(username);
        
        user.TelegramChatId = message.Chat.Id;
        await repository.UpdateAsync(user);
        
        await Bot.SendMessage(message.Chat, $"Linked user: {username} to chat: {message.Chat.Id}", 
            cancellationToken: cancellationToken);
    }
}