using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using LangDiscord.Interfaces;
using LangDiscord.Models;

namespace LangDiscord.Services
{
    public class MyMemoryService(IHttpClientFactory httpClientFactory) : IMyMemoryService
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient("MyMemory");

        public async Task<string> TranslateAsync(string sourceLang, string targetLanguage, string text)
        {
            string encodedText = HttpUtility.UrlEncode(text);
            string url = $"https://api.mymemory.translated.net/get?q={encodedText}&langpair={sourceLang}|{targetLanguage}";

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            string responseBody = await response.Content.ReadAsStringAsync();
            var json = System.Text.Json.JsonDocument.Parse(responseBody);
            var translatedText = json.RootElement
                                     .GetProperty("responseData")
                                     .GetProperty("translatedText")
                                     .GetString();

            if (translatedText == null)
            {
                return "Translation mistake";
            }

            return translatedText;
        }
    }
}
