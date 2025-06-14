using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MiniBoard.Core.Repositories;
using MiniBoard.Core.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MiniBoard.Core.Bot.Services;

public class DueCommandHandler : BaseCommandHandler
{
    private readonly IServiceProvider _serviceProvider;

    public DueCommandHandler(ITelegramBotClient bot, ILogger<DueCommandHandler> logger, IServiceProvider serviceProvider) 
        : base(bot, logger)
    {
        _serviceProvider = serviceProvider;
    }

    protected override Task<bool> CanHandle(Message message)
    {
        return Task.FromResult(message.Text?.Split(' ')[0] == "/due");
    }

    protected override async Task ProcessCommand(Message message, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Processing /due command for chat {ChatId}", message.Chat.Id);

        using var scope = _serviceProvider.CreateScope();
        var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        
        var user = await userRepository.GetByTelegramChatIdAsync(message.Chat.Id);
        if (user == null)
        {
            await Bot.SendMessage(message.Chat, 
                "❌ No linked user found. Please use /link command first to connect your account.", 
                cancellationToken: cancellationToken);
            return;
        }

        var taskService = scope.ServiceProvider.GetRequiredService<IBoardTaskService>();
        
        try
        {
            // Create a mock auth context for the telegram user
            var authContext = scope.ServiceProvider.GetRequiredService<IAuthContextService>();
            if (authContext is AuthContextService authCtx)
            {
                // Set the user context manually for this operation
                authCtx.SetCurrentUser(user.Id, user.Username, user.Email);
            }

            var dueTasks = await taskService.GetTasksDueWithinWeekAsync();
            
            if (!dueTasks.Any())
            {
                await Bot.SendMessage(message.Chat,
                    "📅 No tasks due within the next week.",
                    cancellationToken: cancellationToken);
                return;
            }

            var response = $"📅 <b>Tasks due within the next week ({dueTasks.Count}):</b>\n\n";
            
            foreach (var task in dueTasks)
            {
                var priorityIcon = task.Priority switch
                {
                    Models.Priority.High => "🔴",
                    Models.Priority.Medium => "🟡",
                    Models.Priority.Low => "🟢",
                    _ => "⚪"
                };

                var stateIcon = task.State switch
                {
                    Models.TaskState.ToDo => "📋",
                    Models.TaskState.InProgress => "⚡",
                    Models.TaskState.Review => "👀",
                    Models.TaskState.Done => "✅",
                    Models.TaskState.Backlog => "📦",
                    _ => "❓"
                };

                var dueDateStr = task.DueDate.HasValue ? 
                    task.DueDate.Value.ToString("MMM dd, yyyy") : "No due date";

                response += $"{priorityIcon} {stateIcon} <b>{task.Name}</b>\n";
                response += $"   📅 Due: {dueDateStr}\n";
                if (!string.IsNullOrEmpty(task.Description) && task.Description.Length > 50)
                {
                    response += $"   📝 {task.Description.Substring(0, 50)}...\n";
                }
                else if (!string.IsNullOrEmpty(task.Description))
                {
                    response += $"   📝 {task.Description}\n";
                }
                response += "\n";
            }

            await Bot.SendMessage(message.Chat, response, parseMode: ParseMode.Html, 
                cancellationToken: cancellationToken);
        }
        catch (UnauthorizedAccessException)
        {
            await Bot.SendMessage(message.Chat,
                "❌ Authentication error. Please try linking your account again with /link.",
                cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error processing /due command for chat {ChatId}", message.Chat.Id);
            await Bot.SendMessage(message.Chat,
                "❌ An error occurred while fetching your tasks. Please try again later.",
                cancellationToken: cancellationToken);
        }
    }
}