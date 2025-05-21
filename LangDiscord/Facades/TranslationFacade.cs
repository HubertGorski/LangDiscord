using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Discord;
using Discord.Net;
using LangDiscord.Extensions;
using LangDiscord.Helpers;
using LangDiscord.Interfaces;
using LangDiscord.Models;
using LangDiscord.Services.Cache;

namespace LangDiscord.Facades
{
    public class TranslationFacade(IMessageService messageService, ITatoebaService tatoebaService, IUsersTranslationCache usersTranslationCache, IUserTranslationCache translationCache, IUserFavoriteTranslationCache userFavoriteTranslationCache, IMyMemoryService myMemoryService, ILanguageDetectionService languageDetectionService, IUserSettingsCache userSettingsCache) : ITranslationFacade
    {
        private readonly IMyMemoryService _myMemoryService = myMemoryService;
        private readonly ILanguageDetectionService _languageDetectionService = languageDetectionService;
        private readonly IUserSettingsCache _userSettingsCache = userSettingsCache;
        private readonly IUserFavoriteTranslationCache _userFavoriteTranslationCache = userFavoriteTranslationCache;
        private readonly ITatoebaService _tatoebaService = tatoebaService;
        private readonly IUserTranslationCache _translationCache = translationCache;
        private readonly IUsersTranslationCache _usersTranslationCache = usersTranslationCache;
        private readonly IMessageService _messageService = messageService;

        public async Task HandleGetTranslateContent(UserRequest userRequest)
        {
            if (userRequest.Content == null)
            {
                await MessageHelper.SendMessageToUser(userRequest.Message, "Something went wrong");
                return;
            }

            string? detectedLang = await _languageDetectionService.DetectLanguageAsync(userRequest.Content);

            if (detectedLang == null)
            {
                await MessageHelper.SendMessageToUser(userRequest.Message, "❌ Language detection failed. Use `!langs` to see supported languages.");
                return;
            }

            string targetLang;
            if (LangExtensions.IsValidLanguageCode(userRequest.CommandText))
            {
                targetLang = userRequest.CommandText;
            }
            else
            {
                LanguageUsersSettings userLanguages = _userSettingsCache.GetUserSettings(userRequest.UserId);
                targetLang = userLanguages.MainLanguageCode == detectedLang ? userLanguages.FavoriteLanguageCode : userLanguages.MainLanguageCode;
            }

            var botMessage = await _myMemoryService.TranslateAsync(detectedLang, targetLang, userRequest.Content);

            if (botMessage == null) return;
            await MessageHelper.SendMessageToUser(userRequest.Message, botMessage);
        }

        public async Task HandleGetNewCard(UserRequest userRequest, bool getFromFavorite)
        {
            string selectedLanguageCode = userRequest.CommandText;
            LanguageUsersSettings userLanguages = _userSettingsCache.GetUserSettings(userRequest.UserId);

            TranslationResult result;
            bool isFavoriteMessage = false;
            ulong oldMessageId = 0;
            if (!getFromFavorite)
            {
                result = userLanguages.MainLanguageCode != selectedLanguageCode && userLanguages.FavoriteLanguageCode != selectedLanguageCode
                        ? await _tatoebaService.GetNewCard(userLanguages.MainLanguageCode, selectedLanguageCode)
                        : await _tatoebaService.GetNewCard(userLanguages.MainLanguageCode, userLanguages.FavoriteLanguageCode);
            }
            else
            {
                if (!_userFavoriteTranslationCache.TryGetFavoriteTranslation(userRequest.UserId, userRequest.CommandText, out var translation, out var dictKey) || translation == null)
                {
                    await MessageHelper.SendMessageToUser(userRequest.Message, "🌟 No favorites yet! Save phrases with reactions or commands.");
                    return;
                }

                result = translation.Result;
                oldMessageId = dictKey.Item2;
                await _messageService.RemoveButtonsFromMessage(oldMessageId);
                isFavoriteMessage = true;
            }

            bool isSelectedMainLang = userLanguages.MainLanguageCode == selectedLanguageCode;
            string botMessage = isSelectedMainLang ? result.MainLanguageText : result.OtherLanguageText;
            ulong messageId = await MessageHelper.SendMessageToUser(userRequest.Message, botMessage, isFavoriteMessage);
            if (isFavoriteMessage && oldMessageId != 0)
            {
                _userFavoriteTranslationCache.ReplaceIdTranslation(oldMessageId, messageId);
            }

            TranslationResultForUser resultForUser = new()
            {
                Result = result,
                AnswerInMainLanguage = !isSelectedMainLang
            };

            _translationCache.AddPendingTranslation(userRequest.Message.Author.Id, resultForUser);
            _usersTranslationCache.AddTranslation(messageId, resultForUser);

        }
    }
}
