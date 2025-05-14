using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LangDiscord.Enums;

namespace LangDiscord.Interfaces
{
    public interface ILangCodeConverter
    {
        bool IsValidLanguageCode(string code);
        string GetCode(Lang lang);
        IEnumerable<string> GetAllLangCodes();
        Dictionary<Lang, string> GetLangCodeMap();
        Lang GetLangFromCode(string code);
        string? GetLangCodeFromDetectedLang(string detectedLang);
    }
}
