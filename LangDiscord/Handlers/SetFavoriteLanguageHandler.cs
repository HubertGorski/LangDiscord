using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using LangDiscord.Enums;
using LangDiscord.Facades;
using LangDiscord.Helpers;
using LangDiscord.Interfaces;
using LangDiscord.Models;
using LangDiscord.Services.Cache;

namespace LangDiscord.Handlers
{
    public class SetFavoriteLanguageHandler(ILangCodeConverter langCodeConverter, IUserLangSettingsFacade userLangSettingsFacade) : ICommandHandler
    {
        private readonly ILangCodeConverter _langCodeConverter = langCodeConverter;
        private readonly IUserLangSettingsFacade _userLangSettingsFacade = userLangSettingsFacade;

        public Command SupportedCommand => Command.SET_FAVORITE_LANGUAGE;

        public bool CanExecute(UserRequest request)
            => request.Content != null && _langCodeConverter.IsValidLanguageCode(request.Content);

        public Task ExecuteAsync(UserRequest request)
            => _userLangSettingsFacade.TrySetLanguageAsync(request, false);

        public string ErrorMessage
            => "Error: Invalid language code provided";

        public string HelpMessage
            => "`!fav {langCode}` - Sets your favorite language (the one you're learning)";
    }
}
