using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LangDiscord.Enums;
using LangDiscord.Extensions;
using LangDiscord.Helpers;
using LangDiscord.Interfaces;
using LangDiscord.Models;

namespace LangDiscord.Handlers
{
    public class ShowLangsHandler : ICommandHandler
    {
        public Command SupportedCommand => Command.AVAILABLE_LANGS;

        public bool CanExecute(UserRequest request)
            => request.Content is null;

        public async Task ExecuteAsync(UserRequest request)
        {
            string answer = "\nLanguage - `langCode` \n" + LangExtensions.AllLanguagesWithCodes;
            await MessageHelper.SendMessageToUser(request.Message, answer);
        }
        public string ErrorMessage
            => "Błąd: Ta komenda nie wymaga dodatkowych parametrów";

        public string HelpMessage
            => "`!langs` - Wyświetla listę dostępnych języków";
    }

}
