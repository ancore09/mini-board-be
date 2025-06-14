using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace MiniBoard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BotController: ControllerBase
{
    private readonly ILogger<BotController> _logger;
    private readonly ITelegramBotClient _botClient;

    public BotController(ITelegramBotClient botClient, ILogger<BotController> logger)
    {
        _botClient = botClient;
        _logger = logger;
    }
    
    [HttpPost("send")]
    public async Task<ActionResult> SendMessage([FromBody] SendMessageDto  dto)
    {
        await _botClient.SendMessage(new ChatId(dto.ChatId), dto.Text);
        return Ok();
    }
}

public class SendMessageDto
{
    public string Text { get; set; }
    public long ChatId { get; set; }
}