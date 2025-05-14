using System;
using LangDiscord.Enums;
using LangDiscord.Interfaces;
using LangDiscord.Models;
using LangDiscord.Services.Cache.Common;
using Newtonsoft.Json.Linq;

namespace LangDiscord.Services.Cache
{
    public class UserFavoriteTranslationCache(TimeSpan defaultCacheDuration, TimeSpan cleanupInterval) : IUserFavoriteTranslationCache
    {
        private readonly TimedCache<(ulong, ulong, string), TranslationResultForUser> _innerCache = new(defaultCacheDuration, cleanupInterval);

        public void AddFavoriteTranslation(UserFavorite userFavorite, TimeSpan? customExpiry = null)
            => _innerCache.Set((userFavorite.UserId, userFavorite.MessageId, userFavorite.LangCode), userFavorite.MessageContent, customExpiry);

        public bool TryGetFavoriteTranslation(ulong userId, string langCode, out TranslationResultForUser? result, out (ulong, ulong, string) key)
        {
            var partialKey = (userId, langCode);
            Func<(ulong, ulong, string), (ulong, string), bool> keyMatcher = (key, partial) =>
            key.Item1 == partial.Item1 && key.Item3 == partial.Item2;

            return _innerCache.TryGetRandomByPartialKey(partialKey, keyMatcher, out result, out key);
        }

        public bool ReplaceIdTranslation(ulong oldMessageId, ulong newMessageId)
        {
            Dictionary<(ulong, ulong, string), (TranslationResultForUser Value, DateTime Expiry)> aaa = _innerCache.GetAllEntries();

            if (_innerCache.TryGetRandomByPartialKey(oldMessageId, (key, partial) => key.Item2 == partial, out var oldResult, out var oldKey) && oldResult != null)
            {
                _innerCache.TryRemove((oldKey.Item1, oldKey.Item2, oldKey.Item3), out _);
                _innerCache.Set((oldKey.Item1, newMessageId, oldKey.Item3), oldResult);
                return true;
            }

            return false;
        }

        public bool TryRemoveFavoriteTranslation(UserFavorite userFavorite, out TranslationResultForUser? result)
            => _innerCache.TryRemove((userFavorite.UserId, userFavorite.MessageId, userFavorite.LangCode), out result);

        public int ClearExpired() => _innerCache.ClearExpired();

        public void ClearAllFavoriteTranslations() => _innerCache.Clear();

        public int Count => _innerCache.Count;
    }
}