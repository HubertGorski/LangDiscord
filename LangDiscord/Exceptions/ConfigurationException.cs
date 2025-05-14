using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangDiscord.Exceptions
{
    public class ConfigurationException : Exception
    {
        public string? ConfigurationKey { get; }

        public ConfigurationException(string message)
            : base(message) { }

        public ConfigurationException(string key, string message)
            : base($"Configuration error for '{key}': {message}")
        {
            ConfigurationKey = key;
        }
    }
}
