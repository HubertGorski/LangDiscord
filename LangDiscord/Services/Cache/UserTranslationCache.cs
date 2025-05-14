using System;
using LangDiscord.Interfaces;
using LangDiscord.Models;
using LangDiscord.Services.Cache.Common;

namespace LangDiscord.Services.Cache
{
    public class UserTranslationCache(TimeSpan defaultCacheDuration, TimeSpan cleanupInterval) : IUserTranslationCache
    {
        private readonly TimedCache<ulong, TranslationResultForUser> _innerCache = new(defaultCacheDuration, cleanupInterval);

        public void AddPendingTranslation(ulong userId, TranslationResultForUser result, TimeSpan? customExpiry = null)
            => _innerCache.Set(userId, result, customExpiry);

        public bool TryGetPendingTranslation(ulong userId, out TranslationResultForUser? result)
            => _innerCache.TryGet(userId, out result);

        public bool TryRemovePendingTranslation(ulong userId, out TranslationResultForUser? result)
            => _innerCache.TryRemove(userId, out result);

        public int ClearExpired() => _innerCache.ClearExpired();

        public void ClearAllPendingTranslations() => _innerCache.Clear();

        public int Count => _innerCache.Count;
    }
}