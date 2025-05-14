using Discord;
using LangDiscord.Models;

namespace LangDiscord.Interfaces
{
    public interface IFavoriteService
    {
        void AddFavorite(ulong userId, IUserMessage message);
        bool RemoveFavorite(ulong userId, ulong messageId);
    }
}