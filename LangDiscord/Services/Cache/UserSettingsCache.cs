using System;
using LangDiscord.Enums;
using LangDiscord.Interfaces;
using LangDiscord.Models;
using LangDiscord.Services.Cache.Common;

namespace LangDiscord.Services.Cache
{
    public class UserSettingsCache : IUserSettingsCache
    {
        private readonly TimedCache<ulong, LanguageUsersSettings> _cache;
        private readonly LanguageUsersSettings _defaultSettings;

        public UserSettingsCache(TimeSpan defaultCacheDuration, TimeSpan cleanupInterval)
        {
            _cache = new TimedCache<ulong, LanguageUsersSettings>(defaultCacheDuration, cleanupInterval);
            _defaultSettings = new()
            {
                FavoriteLanguage = Lang.ENGLISH,
                MainLanguage = Lang.POLISH
            };
        }

        public LanguageUsersSettings GetUserSettings(ulong userId)
        {
            return _cache.TryGet(userId, out var settings) && settings != null ? settings : _defaultSettings;
        }

        public void AddOrUpdateSettings(
            ulong userId,
            LanguageUsersSettings settings,
            TimeSpan? customExpiry = null)
        {
            _cache.Set(userId, settings, customExpiry);
        }

        public bool TryRemoveSettings(ulong userId, out LanguageUsersSettings? settings)
        {
            return _cache.TryRemove(userId, out settings);
        }

        public int ClearExpiredSettings() => _cache.ClearExpired();

        public void ClearAllSettings() => _cache.Clear();
    }
}