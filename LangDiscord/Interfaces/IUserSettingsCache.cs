using System;
using LangDiscord.Enums;
using LangDiscord.Models;

namespace LangDiscord.Interfaces
{
    public interface IUserSettingsCache
    {
        LanguageUsersSettings GetUserSettings(ulong userId);
        void AddOrUpdateSettings(ulong userId, LanguageUsersSettings settings, TimeSpan? customExpiry = null);
        bool TryRemoveSettings(ulong userId, out LanguageUsersSettings? settings);
        int ClearExpiredSettings();
        void ClearAllSettings();
    }
}