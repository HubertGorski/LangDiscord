using System.Text.Json;
using LangDiscord.Interfaces;
using LangDiscord.Models;

namespace LangDiscord.Services;

public record TatoebaApiResponse(
    List<TatoebaSentence> Data
);

public record TatoebaSentence(
    int Id,
    string Text,
    string Lang,
    string? Script,
    string License,
    List<List<TatoebaTranslation>> Translations
);

public record TatoebaTranslation(
    int Id,
    string Text,
    string Lang,
    string? Script,
    string License,
    List<object> Transcriptions,
    List<TatoebaAudio> Audios,
    string Owner
);

public record TatoebaAudio(
    string Author,
    string AttributionUrl,
    string License,
    string DownloadUrl
);

public class TatoebaService(IHttpClientFactory httpClientFactory) : ITatoebaService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("Tatoeba");

    public async Task<TranslationResult> GetNewCard(string mainLanguage, string otherLanguage)
    {
        var url = $"https://api.tatoeba.org/unstable/sentences?lang={mainLanguage}&sort=random";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var apiResponse = JsonSerializer.Deserialize<TatoebaApiResponse>(responseString, options);

        if (apiResponse?.Data == null || apiResponse.Data.Count == 0)
            throw new Exception("No sentences found");

        foreach (var sentence in apiResponse.Data)
        {
            var translation = FindTranslation(sentence.Translations, otherLanguage);
            if (translation != null && sentence.Lang == mainLanguage)
            {
                return new TranslationResult
                {
                    OtherLanguageText = translation,
                    MainLanguageText = sentence.Text,
                    MainLanguageCode = mainLanguage,
                    OtherLanguageCode = otherLanguage
                };
            }
        }

        throw new Exception("No matching translation found");
    }

    private static string? FindTranslation(List<List<TatoebaTranslation>>? translations, string targetLang)
    {
        return translations?
            .SelectMany(g => g)
            .FirstOrDefault(t => t.Lang == targetLang)?
            .Text;
    }
}