using LangDiscord.Models;

namespace LangDiscord.Interfaces
{
    public interface ITatoebaService
    {
        Task<TranslationResult> GetNewCard(string mainLanguage, string otherLanguage);
    }
}