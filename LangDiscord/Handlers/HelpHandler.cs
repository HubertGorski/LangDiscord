using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LangDiscord.Enums;
using LangDiscord.Helpers;
using LangDiscord.Interfaces;
using LangDiscord.Models;
using LangDiscord.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LangDiscord.Handlers
{
    public class HelpHandler(IServiceProvider serviceProvider) : ICommandHandler
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        public Command SupportedCommand => Command.HELP;

        public bool CanExecute(UserRequest request)
            => request.Content is null;

        public async Task ExecuteAsync(UserRequest request)
        {
            using var scope = _serviceProvider.CreateScope();
            var handlers = scope.ServiceProvider
                              .GetServices<ICommandHandler>()
                              .Where(h => h.GetType() != typeof(HelpHandler));

            var helpText = "\n# Available commands: \n" +
                HelpMessage + "\n" +
                string.Join("\n\n",
                handlers.Select(h => h.HelpMessage)) +
                "\n \n`❤️` Adding a heart reaction to a message saves it to favorites." +
                "\n## Legend: \n" +
                "`{langCode}` - Language code, e.g. pol, spa, eng \n" +
                "`{text}` - Text to translate \n";

            await MessageHelper.SendMessageToUser(request.Message, helpText);
        }

        public string ErrorMessage
            => "Error: This command doesn't require additional parameters";

        public string HelpMessage
            => "`!help` - Displays all available bot commands.";
    }
}
