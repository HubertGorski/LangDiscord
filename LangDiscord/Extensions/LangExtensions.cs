using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LangDiscord.Enums;
using LangDiscord.Interfaces;

namespace LangDiscord.Extensions
{
    public static class LangExtensions
    {
        private static readonly LangCodeConverter _converter = new();

        public static string GetLanguageCode(this Lang lang)
            => _converter.GetCode(lang);

        public static bool IsValidLanguageCode(this string code)
            => _converter.IsValidLanguageCode(code);

        public static string AllLanguagesWithCodes
            => string.Join(Environment.NewLine, _converter.GetLangCodeMap().Select(kv =>
            {
                string langName = kv.Key.ToString();
                string formattedName = char.ToUpper(langName[0]) + langName.Substring(1).ToLower();
                return $"{formattedName} - `{kv.Value}`";
            })
    );
    }
}
