using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using LangDiscord.Enums;
using LangDiscord.Helpers;
using LangDiscord.Interfaces;
using LangDiscord.Models;
using LangDiscord.Services.Cache;

namespace LangDiscord.Handlers
{
    public class CheckConfigHandler(IUserSettingsCache userSettingsCache) : ICommandHandler
    {
        private readonly IUserSettingsCache _userSettingsCache = userSettingsCache;

        public Command SupportedCommand => Command.CHECK_CONFIG;

        public bool CanExecute(UserRequest request)
            => request.Content is null;

        public async Task ExecuteAsync(UserRequest request)
        {
            LanguageUsersSettings settings = _userSettingsCache.GetUserSettings(request.UserId);

            await MessageHelper.SendMessageToUser(request.Message, $"Język główny: {settings.MainLanguage}; " +
                $"Ulubiony język: {settings.FavoriteLanguage}");
        }

        public string ErrorMessage
            => "Błąd: Ta komenda nie wymaga dodatkowych parametrów";

        public string HelpMessage
            => "`!config` - Wyświetla aktualne ustawienia: język główny (domyślny), ulubiony język (do szybkich tłumaczeń)";
    }
}
