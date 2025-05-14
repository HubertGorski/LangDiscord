using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangDiscord.Models
{
    public class TranslationResultForUser
    {
        public required TranslationResult Result { get; set; }

        public required bool AnswerInMainLanguage { get; set; }

        public string Answer => AnswerInMainLanguage ? Result.MainLanguageText : Result.OtherLanguageText;

        public string LangCode => AnswerInMainLanguage ? Result.OtherLanguageCode : Result.MainLanguageCode;

        public string RevertLangCode => AnswerInMainLanguage ? Result.MainLanguageCode : Result.OtherLanguageCode;

        public bool IsAnswerCard { get; set; } = false;
    }
}
