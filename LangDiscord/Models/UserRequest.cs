using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using LangDiscord.Enums;

namespace LangDiscord.Models
{
    public class UserRequest
    {
        public required ulong UserId { get; set; }
        public required SocketMessage Message { get; set; }
        public required string CommandText { get; set; }
        public required string? Content { get; set; }

    }
}
