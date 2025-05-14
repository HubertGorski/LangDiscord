using LangDiscord.Models;

namespace LangDiscord.Interfaces
{
    public interface IUsersTranslationCache
    {
        int Count { get; }

        void AddTranslation(ulong messageId, TranslationResultForUser translation, TimeSpan? customExpiry = null);
        void ClearAllFavoriteTranslations();
        int ClearExpired();
        bool TryGetTranslation(ulong messageId, out TranslationResultForUser? result);

    }
}