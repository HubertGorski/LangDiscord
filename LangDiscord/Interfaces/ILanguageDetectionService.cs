namespace LangDiscord.Interfaces
{
    public interface ILanguageDetectionService
    {
        Task<string?> DetectLanguageAsync(string text);
    }
}