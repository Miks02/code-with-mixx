using CodeWithMixx.Common.Results;

namespace CodeWithMixx.Domain.Entities.Discord;

public static class DiscordError
{
    public static Error ChannelNotFound(ulong? channelId = null)
    {
        return channelId is null
            ? new Error("ChannelNotFound", "Channel not found")
            : new Error("ChannelNotFound", $"Channel with id {channelId} not found");
    }
    
    
      
}