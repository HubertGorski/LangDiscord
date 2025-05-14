using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LangDiscord.Enums;
using LangDiscord.Models;

namespace LangDiscord.Interfaces
{
    public interface ICommandExecutorService
    {
        Task Execute(UserRequest request);
    }
}
