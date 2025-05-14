using Discord;

namespace LangDiscord.Interfaces
{
    public interface IMessageService
    {
        Task<IMessage?> GetMessageFromCacheOrApiAsync(ulong messageId);
        Task RemoveButtonsFromMessage(ulong messageId);
    }
}