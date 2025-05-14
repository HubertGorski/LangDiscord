using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Net;
using Discord.WebSocket;
using LangDiscord.Enums;
using LangDiscord.Extensions;
using LangDiscord.Facades;
using LangDiscord.Helpers;
using LangDiscord.Interfaces;
using LangDiscord.Models;
using LangDiscord.Services;
using LangDiscord.Services.Cache;

namespace LangDiscord.Handlers
{

    public class LangActionHandler(ITranslationFacade translationFacade) : ICommandHandler
    {

        private readonly ITranslationFacade _translationFacade = translationFacade;

        public Command SupportedCommand => Command.LANG_ACTION;

        public bool CanExecute(UserRequest request)
            => LangExtensions.IsValidLanguageCode(request.CommandText);

        public async Task ExecuteAsync(UserRequest request)
        {
            if (request.Content == null)
            {
                await _translationFacade.HandleGetNewCard(request, false);
                return;
            }

            if (request.Content == "u")
            {
                await _translationFacade.HandleGetNewCard(request, true);
                return;
            }

            await _translationFacade.HandleGetTranslateContent(request);
        }

        public string ErrorMessage
            => "Błąd: Podano niewłaściwy kod języka";

        public string HelpMessage
            => "`!{langCode} {text}` - Wyświetla tłumaczenie tekstu w podanym języku. \n" +
                "`!{langCode}` - Wyświetla fiszkę o podanym języku a następnie oczekuje na odpowiedź. \n" +
                "`!{langCode} u` - Wyświetla fiszkę z ulubionych o podanym języku a następnie oczekuje na odpowiedź. \n" +
                "W odpowiedzi podaje tłumaczenie wpisanego tekstu w języku głównym.Jeśli tekst jest w języku głównym, podaje tłumaczenie w ulubionym języku.";
    }
}
