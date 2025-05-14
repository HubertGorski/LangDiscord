using LangDiscord.Enums;
using LangDiscord.Models;

namespace LangDiscord.Interfaces
{
    public interface IUserFavoriteTranslationCache
    {
        int Count { get; }

        void AddFavoriteTranslation(UserFavorite userFavorite, TimeSpan? customExpiry = null);
        void ClearAllFavoriteTranslations();
        int ClearExpired();
        bool TryGetFavoriteTranslation(ulong userId, string langCode, out TranslationResultForUser? result, out (ulong, ulong, string) key);
        bool TryRemoveFavoriteTranslation(UserFavorite userFavorite, out TranslationResultForUser? result);

        bool ReplaceIdTranslation(ulong oldMessageId, ulong newMessageId);

    }
}