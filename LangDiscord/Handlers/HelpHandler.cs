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

            var helpText = "\n# Dostępne komendy: \n" +
                HelpMessage + "\n" +
                string.Join("\n\n",
                handlers.Select(h => h.HelpMessage)) +
                "\n \n`❤️` Dodanie reakcji serca do wiadomości zapisuje fiszkę w ulubionych." +
                "\n## Legenda: \n" +
                "`{langCode}` - Kod języka, np. pol, spa, eng. \n" +
                "`{text}` - Tekst do przetłumaczenia \n";

            await MessageHelper.SendMessageToUser(request.Message, helpText);
        }

        public string ErrorMessage
            => "Błąd: Ta komenda nie wymaga dodatkowych parametrów";

        public string HelpMessage
            => "`!help` - Przedstawia wszystkie dozwolone komendy bota.";
    }
}
