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
             => "Error: Cannot display translation. Missing text input.";

        public string HelpMessage
            => "`!! {text}` - Returns translation of the given text in your main language. " +
                "If the provided text is already in your main language, returns translation in your favorite language.";
    }
}
