using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace MiniBoard.Core.Bot.Services;

public class DefaultCommandHandler : BaseCommandHandler
{
    public DefaultCommandHandler(ITelegramBotClient bot, ILogger<DefaultCommandHandler> logger) 
        : base(bot, logger)
    {
    }

    protected override Task<bool> CanHandle(Message message)
    {
        return Task.FromResult(true);
    }

    protected override async Task ProcessCommand(Message message, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Processing default command for user {Username} in chat {ChatId}", 
            message.Chat.Username, message.Chat.Id);

        const string usage = """
                                 <b><u>MiniBoard menu</u></b>:
                                 /link           - link your telegram account
                                 /due            - show tasks due within a week
                             """;
        
        await Bot.SendMessage(message.Chat, usage, parseMode: ParseMode.Html, 
            replyMarkup: new ReplyKeyboardRemove(), cancellationToken: cancellationToken);
    }
}