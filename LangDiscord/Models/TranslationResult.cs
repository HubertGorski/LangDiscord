using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LangDiscord.Enums;

namespace LangDiscord.Models
{
    public class TranslationResult
    {
        public required string OtherLanguageText { get; set; }

        public required string MainLanguageText { get; set; }

        public required string OtherLanguageCode { get; set; }

        public required string MainLanguageCode { get; set; }

    }
}
