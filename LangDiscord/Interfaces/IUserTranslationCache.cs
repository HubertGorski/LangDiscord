using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LangDiscord.Models;

namespace LangDiscord.Interfaces
{
    public interface IUserTranslationCache
    {
        void AddPendingTranslation(ulong userId, TranslationResultForUser result, TimeSpan? customExpiry = null);
        bool TryGetPendingTranslation(ulong userId, out TranslationResultForUser? result);
        bool TryRemovePendingTranslation(ulong userId, out TranslationResultForUser? result);
        int ClearExpired();
    }
}
