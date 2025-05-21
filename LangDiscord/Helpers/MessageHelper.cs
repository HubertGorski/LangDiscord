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
using LangDiscord.Models;

namespace LangDiscord.Helpers
{
    public static class MessageHelper
    {

        public static async Task<ulong> SendMessageToUser(SocketMessage message, string text, bool isFavoriteMessage = false)
        {
            if (isFavoriteMessage)
            {
                var button = new ComponentBuilder()
                    .WithButton("🗑️ Remove from favorites", $"remove_fav:{message.Author.Id}", ButtonStyle.Primary)
                    .Build();
                var sentFavoriteMessage = await message.Channel.SendMessageAsync($"[{message.Author.GlobalName}] - {text}", components: button);
                return sentFavoriteMessage.Id;
            }

            var sentMessage = await message.Channel.SendMessageAsync($"[{message.Author.GlobalName}] - {text}");
            return sentMessage.Id;
        }
    }
}
