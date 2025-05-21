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
            => "Error: This command doesn't accept additional parameters";

        public string HelpMessage
            => "`!langs` - Displays the list of available languages";
    }

}
