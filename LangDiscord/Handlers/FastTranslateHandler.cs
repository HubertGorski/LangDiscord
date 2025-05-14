using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Net;
using LangDiscord.Enums;
using LangDiscord.Facades;
using LangDiscord.Helpers;
using LangDiscord.Interfaces;
using LangDiscord.Models;
using LangDiscord.Services.Cache;

namespace LangDiscord.Handlers
{
    public class FastTranslateHandler(ITranslationFacade translationFacade) : ICommandHandler
    {
        private readonly ITranslationFacade _translationFacade = translationFacade;

        public Command SupportedCommand => Command.FAST_TRANSLATE;

        public bool CanExecute(UserRequest request)
            => request.Content != null;

        public async Task ExecuteAsync(UserRequest request)
        => await _translationFacade.HandleGetTranslateContent(request);

        public string ErrorMessage
            => "Błąd: Nie można wyświetlić tłumaczenia. Brakuje tekstu.";

        public string HelpMessage
            => "`!! {text}` - Zwraca tłumaczenie podanego tekstu w języku głównym. " +
                "Jeśli podany tekst jest w języku głównym, zwraca tłumaczenie w ulubionym języku.";
    }
}
