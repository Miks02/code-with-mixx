using Discord;
using Discord.WebSocket;

namespace CodeWithMixx.Infrastructure.BackgroundJobs;

public class DiscordBotService : BackgroundService
{
    private readonly ILogger<DiscordBotService> _logger;
    private readonly DiscordSocketClient _client;
    private readonly IConfiguration _configuration;

    public DiscordBotService(ILogger<DiscordBotService> logger, IConfiguration configuration, DiscordSocketClient client)
    {
        _logger = logger;
        _configuration = configuration;
        _client = client;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _client.Log += LogAsync;

        _client.Ready += () =>
        {
            _logger.LogInformation("CodeWithMixx Bot is online!");
            return Task.CompletedTask;
        };
        
        await _client.LoginAsync(TokenType.Bot, _configuration["Discord:BotToken"]);
        await _client.StartAsync();
        
        await Task.Delay(-1, stoppingToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await _client.StopAsync();
        await _client.LogoutAsync();
        await base.StopAsync(cancellationToken);
    }

    private Task LogAsync(LogMessage log)
    {
        _logger.LogInformation("[Discord]: {LogMessage}", log.Message);
        return Task.CompletedTask;
    }
}