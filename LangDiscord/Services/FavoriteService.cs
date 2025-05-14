using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Discord;
using LangDiscord.Extensions;
using LangDiscord.Interfaces;
using LangDiscord.Models;
using LangDiscord.Services.Cache;

namespace LangDiscord.Services
{
    public class FavoriteService(IUserFavoriteTranslationCache userFavoriteTranslationCache, IUsersTranslationCache usersTranslationCache) : IFavoriteService
    {
        private readonly IUserFavoriteTranslationCache _userFavoriteTranslationCache = userFavoriteTranslationCache;
        private readonly IUsersTranslationCache _usersTranslationCache = usersTranslationCache;

        public void AddFavorite(ulong userId, IUserMessage message)
        {
            if (_usersTranslationCache.TryGetTranslation(message.Id, out var content) && content != null)
            {
                _userFavoriteTranslationCache.AddFavoriteTranslation(new UserFavorite
                {
                    UserId = userId,
                    MessageId = message.Id,
                    LangCode = content.IsAnswerCard ? content.RevertLangCode : content.LangCode,
                    MessageContent = content
                });
            }
        }

        public bool RemoveFavorite(ulong userId, ulong messageId)
        {
            if (_usersTranslationCache.TryGetTranslation(messageId, out var content) && content != null)
            {
                return _userFavoriteTranslationCache.TryRemoveFavoriteTranslation(new UserFavorite
                {
                    UserId = userId,
                    MessageId = messageId,
                    LangCode = content.IsAnswerCard ? content.RevertLangCode : content.LangCode,
                    MessageContent = content
                }, out var _);
            }

            return false;
        }
    }
}
