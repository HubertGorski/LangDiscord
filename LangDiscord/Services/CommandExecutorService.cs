using System;
using System.Collections.Generic;
using LangDiscord.Enums;
using LangDiscord.Handlers;
using LangDiscord.Helpers;
using LangDiscord.Interfaces;
using LangDiscord.Models;

namespace LangDiscord.Services
{
    public class CommandExecutorService(IEnumerable<ICommandHandler> handlers, ICommandExtensions commandExt) : ICommandExecutorService
    {
        private readonly ICommandExtensions _commandExt = commandExt;
        private readonly Dictionary<Command, ICommandHandler> _handlers = handlers?.ToDictionary(h => h.SupportedCommand)
                ?? throw new ArgumentNullException(nameof(handlers));

        public async Task Execute(UserRequest request)
        {
            Command? command = _commandExt.GetCommandFromString(request.CommandText);
            if (!command.HasValue || !_handlers.TryGetValue(command.Value, out var handler))
            {
                await MessageHelper.SendMessageToUser(request.Message, "Error: Invalid command or language code");
                return;
            }

            if (handler.CanExecute(request))
            {
                await handler.ExecuteAsync(request);
            }
            else
            {
                await MessageHelper.SendMessageToUser(request.Message, handler.ErrorMessage);
            }
        }

    }
}