using Discord.WebSocket;
using LangDiscord.Models;

namespace LangDiscord.Interfaces
{
    public interface IUserLangSettingsFacade
    {
        Task<bool> TrySetLanguageAsync(UserRequest request, bool isMainLanguage);
    }
}