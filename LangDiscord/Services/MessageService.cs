using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Discord.Net;
using Discord.WebSocket;
using LangDiscord.Interfaces;
using LangDiscord.Models;

namespace LangDiscord.Services
{
    public class MessageService(DiscordSocketClient discordClient, DiscordData discordData) : IMessageService
    {
        private readonly DiscordSocketClient _discordClient = discordClient;
        private readonly DiscordData _discordData = discordData;

        public async Task<IMessage?> GetMessageFromCacheOrApiAsync(ulong messageId)
        {
            try
            {
                if (await _discordClient.GetChannelAsync(_discordData.ChannelId) is not SocketTextChannel channel)
                {
                    Console.WriteLine("Channel does not exist or is not a text channel"); // TODO: https://dev.azure.com/hubertgorski181/HubProjects/_workitems/edit/2
                    return null;
                }

                return await channel.GetMessageAsync(messageId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message retrieval error: {ex.Message}");
                return null;
            }
        }

        public async Task RemoveButtonsFromMessage(ulong messageId)
        {
            var message = await GetMessageFromCacheOrApiAsync(messageId);

            if (message == null)
            {
                Console.WriteLine("Message not found");
                return;
            }

            if (message is not IUserMessage userMessage)
            {
                Console.WriteLine("Message cannot be edited");
                return;
            }

            try
            {
                await userMessage.ModifyAsync(msg =>
                {
                    msg.Components = new ComponentBuilder().Build();
                    msg.Content = userMessage.Content;
                });

                Console.WriteLine($"Successfully removed buttons from message with ID: {message.Id}");
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Error: No permission to modify the message!");
            }
            catch (HttpException httpEx) when (httpEx.DiscordCode == DiscordErrorCode.UnknownMessage)
            {
                Console.WriteLine("This message doesn't exist or was removed!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.GetType().Name}: {ex.Message}");
            }
        }
    }
}
