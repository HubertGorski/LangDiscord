using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Text.Json;
using LangDiscord.Interfaces;
using DetectLanguage;
using LangDiscord.Helpers;
using Microsoft.Extensions.Configuration;
using LangDiscord.Enums;

namespace LangDiscord.Services
{
    public class LanguageDetectionService : ILanguageDetectionService
    {
        private readonly DetectLanguageClient _client;
        private static readonly LangCodeConverter _converter = new();

        public LanguageDetectionService(IConfiguration config)
        {
            var apiKey = ConfigHelper.GetDetectLangApiKey(config);
            _client = new DetectLanguageClient(apiKey);
        }

        public async Task<string?> DetectLanguageAsync(string text)
        {
            var result = await _client.DetectAsync(text);
            string detectedLang = result[0].language;
            return _converter.GetLangCodeFromDetectedLang(detectedLang);
        }
    }

}
