using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord;
using LangDiscord.Exceptions;
using LangDiscord.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace LangDiscord.Helpers
{
    internal static class ConfigHelper
    {
        public static string GetDetectLangApiKey(IConfiguration config)
        {
            return GetRequiredConfigValue(config, "DetectLangApiKey");
        }

        public static DiscordData GetDiscordData(IConfiguration config)
        {
            string token = GetRequiredConfigValue(config, "DiscordToken");
            string channelIdStr = GetRequiredConfigValue(config, "DiscordChannelId");

            if (!ulong.TryParse(channelIdStr, out ulong channelId) || channelId == 0)
            {
                throw new ConfigurationException("DiscordChannelId", $"Invalid format: '{channelIdStr}'. Must be a valid non-zero ulong.");
            }

            return new DiscordData
            {
                Token = token,
                ChannelId = channelId
            };
        }

        private static string GetRequiredConfigValue(IConfiguration config, string key)
        {
            var value = config[key]?.Trim();
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ConfigurationException(key, "is missing or empty in configuration");
            }
            return value;
        }

        public static TimeSpan GetCacheDuration(IConfiguration config, string configKey)
        {
            string cacheDurationStr = GetRequiredConfigValue(config, configKey);
            if (!TimeSpan.TryParse(cacheDurationStr, out TimeSpan cacheDuration))
            {
                throw new ConfigurationException(
                    configKey,
                    $"Invalid TimeSpan format: '{cacheDurationStr}'. Expected format: 'hh:mm:ss' or 'd.hh:mm:ss'");
            }

            if (cacheDuration <= TimeSpan.Zero)
            {
                throw new ConfigurationException(
                    configKey,
                    $"Cache duration must be positive. Provided value: '{cacheDurationStr}'");
            }

            return cacheDuration;
        }

        public static DiscordSocketConfig GetDiscordSocketConfig()
        {
            return new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.Guilds
                               | GatewayIntents.GuildMessages
                               | GatewayIntents.GuildMessageReactions
                               | GatewayIntents.DirectMessageReactions
                               | GatewayIntents.AllUnprivileged
                               | GatewayIntents.MessageContent,
                LogLevel = LogSeverity.Info,
            };
        }
    }
}
