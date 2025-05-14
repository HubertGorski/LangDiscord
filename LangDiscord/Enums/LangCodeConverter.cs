using System;
using System.Collections.Generic;
using LangDiscord.Interfaces;

namespace LangDiscord.Enums
{
    public class LangCodeConverter : ILangCodeConverter
    {
        private static readonly Dictionary<Lang, string> LangCodeMap = new()
        {
            { Lang.ENGLISH, "eng" },
            { Lang.POLISH, "pol" },
            { Lang.SPANISH, "spa" }
        };

        private static readonly Dictionary<string, string> DetectedCodeMapper = new()
        {
            {"en", "eng"},
            {"pl", "pol"},
            {"es", "spa"}
        };

        public string? GetLangCodeFromDetectedLang(string detectedLang)
            => DetectedCodeMapper.TryGetValue(detectedLang, out var code) ? code : null;

        public bool IsValidLanguageCode(string code)
        {
            return LangCodeMap.ContainsValue(code);
        }

        public string GetCode(Lang lang)
            => LangCodeMap[lang];

        public Dictionary<Lang, string> GetLangCodeMap()
            => LangCodeMap;

        public IEnumerable<string> GetAllLangCodes()
            => LangCodeMap.Values;

        public Lang GetLangFromCode(string code)
        {
            foreach (var pair in LangCodeMap)
            {
                if (pair.Value.Equals(code, StringComparison.OrdinalIgnoreCase))
                    return pair.Key;
            }
            throw new ArgumentException($"Unknown language code: {code}");
        }
    }
}