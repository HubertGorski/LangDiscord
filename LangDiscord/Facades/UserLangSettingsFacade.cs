using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using LangDiscord.Helpers;
using LangDiscord.Interfaces;
using LangDiscord.Models;

namespace LangDiscord.Facades
{
    public class UserLangSettingsFacade(ILangCodeConverter langCodeConverter, IUserSettingsCache userSettingsCache) : IUserLangSettingsFacade
    {
        private readonly ILangCodeConverter _langCodeConverter = langCodeConverter;
        private readonly IUserSettingsCache _userSettingsCache = userSettingsCache;

        public async Task<bool> TrySetLanguageAsync(UserRequest request, bool isMainLanguage)
        {
            var settings = _userSettingsCache.GetUserSettings(request.UserId);
            string? langCode = request.Content;

            if (langCode == null)
            {
                await MessageHelper.SendMessageToUser(request.Message, "Error! Invalid language code provided");
                return false;
            }

            if (isMainLanguage && langCode == settings.FavoriteLanguageCode)
            {
                await MessageHelper.SendMessageToUser(request.Message, "Error! Main language cannot be the same as favorite language");
                return false;
            }

            if (!isMainLanguage && langCode == settings.MainLanguageCode)
            {
                await MessageHelper.SendMessageToUser(request.Message, "Error! Favorite language cannot be the same as main language");
                return false;
            }

            if (isMainLanguage)
                settings.MainLanguage = _langCodeConverter.GetLangFromCode(langCode);
            else
                settings.FavoriteLanguage = _langCodeConverter.GetLangFromCode(langCode);

            _userSettingsCache.AddOrUpdateSettings(request.UserId, settings);

            var languageType = isMainLanguage ? "main" : "favorite";
            await MessageHelper.SendMessageToUser(
                request.Message,
                $"{languageType} language set to: {_langCodeConverter.GetLangFromCode(langCode)} ({langCode})");

            return true;
        }
    }
}