using System;
using LangDiscord.Interfaces;
using LangDiscord.Models;
using LangDiscord.Services.Cache.Common;

namespace LangDiscord.Services.Cache
{
    public class UsersTranslationCache(TimeSpan defaultCacheDuration, TimeSpan cleanupInterval) : IUsersTranslationCache
    {
        private readonly TimedCache<(ulong, string), TranslationResultForUser> _innerCache = new(defaultCacheDuration, cleanupInterval);

        public void AddTranslation(ulong messageId, TranslationResultForUser translation, TimeSpan? customExpiry = null)
            => _innerCache.Set((messageId, translation.IsAnswerCard ? translation.RevertLangCode : translation.LangCode), translation, customExpiry);

        public bool TryGetTranslation(ulong messageId, out TranslationResultForUser? result)
        {
            Func<(ulong messageId, string languageCode), ulong, bool> keyMatcher = (key, partial) =>
                key.messageId == partial;
            Dictionary<(ulong, string), (TranslationResultForUser Value, DateTime Expiry)> aaa = _innerCache.GetAllEntries();
            return _innerCache.TryGetRandomByPartialKey(messageId, keyMatcher, out result, out _);
        }

        public int ClearExpired() => _innerCache.ClearExpired();

        public void ClearAllFavoriteTranslations() => _innerCache.Clear();

        public int Count => _innerCache.Count;
    }
}