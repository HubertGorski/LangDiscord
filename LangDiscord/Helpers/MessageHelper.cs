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
                    .WithButton("🗑️ Usuń z ulubionych", $"remove_fav:{message.Author.Id}", ButtonStyle.Primary)
                    .Build();
                var sentMessage2 = await message.Channel.SendMessageAsync($"[{message.Author.GlobalName}] - {text}", components: button);
                return sentMessage2.Id;
            }

            var sentMessage = await message.Channel.SendMessageAsync($"[{message.Author.GlobalName}] - {text}");
            return sentMessage.Id;
        }
    }
}
