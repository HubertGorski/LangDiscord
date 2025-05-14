using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LangDiscord.Enums;
using LangDiscord.Models;

namespace LangDiscord.Interfaces
{
    public interface ICommandHandler
    {
        Command SupportedCommand { get; }
        bool CanExecute(UserRequest request);
        Task ExecuteAsync(UserRequest request);
        string ErrorMessage { get; }
        string HelpMessage { get; }
    }
}
