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
                    Console.WriteLine("Kanał nie istnieje lub nie jest kanałem tekstowym");
                    return null;
                }

                return await channel.GetMessageAsync(messageId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas pobierania wiadomości: {ex.Message}");
                return null;
            }
        }

        public async Task RemoveButtonsFromMessage(ulong messageId)
        {
            var message = await GetMessageFromCacheOrApiAsync(messageId);

            if (message == null)
            {
                Console.WriteLine("Nie znaleziono wiadomości");
                return;
            }

            if (message is not IUserMessage userMessage)
            {
                Console.WriteLine("Wiadomość nie może być zmodyfikowana (nie jest IUserMessage)");
                return;
            }

            try
            {
                await userMessage.ModifyAsync(msg =>
                {
                    msg.Components = new ComponentBuilder().Build();
                    msg.Content = userMessage.Content;
                });

                Console.WriteLine($"Pomyślnie usunięto przyciski z wiadomości o ID: {message.Id}");
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Błąd: Brak uprawnień do modyfikacji wiadomości!");
            }
            catch (HttpException httpEx) when (httpEx.DiscordCode == DiscordErrorCode.UnknownMessage)
            {
                Console.WriteLine("Błąd: Wiadomość nie istnieje lub została usunięta!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Nieoczekiwany błąd: {ex.GetType().Name}: {ex.Message}");
            }
        }
    }
}
