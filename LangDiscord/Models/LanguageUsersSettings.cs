using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LangDiscord.Enums;
using LangDiscord.Extensions;

namespace LangDiscord.Models
{
    public class LanguageUsersSettings
    {
        public required Lang FavoriteLanguage { get; set; }

        public required Lang MainLanguage { get; set; }

        public string MainLanguageCode => MainLanguage.GetLanguageCode();

        public string FavoriteLanguageCode => FavoriteLanguage.GetLanguageCode();
    }
}
