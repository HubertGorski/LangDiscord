using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using LangDiscord.Enums;
using LangDiscord.Facades;
using LangDiscord.Helpers;
using LangDiscord.Interfaces;
using LangDiscord.Models;
using LangDiscord.Services.Cache;

namespace LangDiscord.Handlers
{
    public class SetMainLanguageHandler(ILangCodeConverter langCodeConverter, IUserLangSettingsFacade userLangSettingsFacade) : ICommandHandler
    {
        private readonly ILangCodeConverter _langCodeConverter = langCodeConverter;
        private readonly IUserLangSettingsFacade _userLangSettingsFacade = userLangSettingsFacade;

        public Command SupportedCommand => Command.SET_MAIN_LANGUAGE;

        public bool CanExecute(UserRequest request)
            => request.Content != null && _langCodeConverter.IsValidLanguageCode(request.Content);

        public Task ExecuteAsync(UserRequest request)
            => _userLangSettingsFacade.TrySetLanguageAsync(request, true);

        public string ErrorMessage
            => "Error: Invalid language code provided";

        public string HelpMessage
            => "`!main {langCode}` - Sets your main language (default/native language)";
    }
}
