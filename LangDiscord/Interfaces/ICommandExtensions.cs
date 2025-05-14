using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LangDiscord.Enums;

namespace LangDiscord.Interfaces
{
    public interface ICommandExtensions
    {
        bool DoesCommandExist(string command);
        Command? GetCommandFromString(string command);
    }
}
