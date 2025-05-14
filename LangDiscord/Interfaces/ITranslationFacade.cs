using LangDiscord.Models;

namespace LangDiscord.Interfaces
{
    public interface ITranslationFacade
    {
        Task HandleGetTranslateContent(UserRequest userRequest);
        Task HandleGetNewCard(UserRequest userRequest, bool getFromFavorite);
    }
}