namespace LangDiscord.Interfaces
{
    public interface IMyMemoryService
    {
        Task<string> TranslateAsync(string sourceLang, string targetLanguage, string text);
    }
}