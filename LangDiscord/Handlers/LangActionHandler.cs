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
            => "Error: Invalid language code provided";

        public string HelpMessage
            => "`!{langCode} {text}` - Translates text to the specified language. \n" +
               "`!{langCode}` - Displays a flashcard in the given language and waits for your response. \n" +
               "`!{langCode} u` - Displays a favorite flashcard in the given language and waits for your response. \n" +
               "The response provides translation to your main language. If text is already in your main language, shows translation in your favorite language.";
    }
}
