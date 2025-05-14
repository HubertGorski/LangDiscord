// CommandExtensions.cs
using System.Collections.Generic;
using LangDiscord.Enums;
using LangDiscord.Interfaces;
using LangDiscord.Models;

namespace LangDiscord.Extensions
{
    public class CommandExtensions : ICommandExtensions
    {
        private readonly Dictionary<Command, string> _commandMap;
        private readonly ILangCodeConverter _langCodeConverter;

        public CommandExtensions(ILangCodeConverter langCodeConverter)
        {
            _langCodeConverter = langCodeConverter;
            _commandMap = new()
            {
                { Command.HELP, "help" },
                { Command.SET_MAIN_LANGUAGE, "main" },
                { Command.SET_FAVORITE_LANGUAGE, "fav" },
                { Command.CHECK_CONFIG, "config" },
                { Command.AVAILABLE_LANGS, "langs" },
                { Command.FAST_TRANSLATE, "!" }
            };
        }

        public bool DoesCommandExist(string command)
        {
            return _commandMap.ContainsValue(command) ||
                   _langCodeConverter.IsValidLanguageCode(command);
        }

        public Command? GetCommandFromString(string command)
        {
            foreach (var pair in _commandMap)
            {
                if (pair.Value == command)
                    return pair.Key;
            }

            if (_langCodeConverter.IsValidLanguageCode(command))
                return Command.LANG_ACTION;

            return null;
        }
    }
}