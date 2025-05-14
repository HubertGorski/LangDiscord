using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LangDiscord.Enums;

namespace LangDiscord.Models
{
    public class UserFavorite
    {
        public required ulong UserId { get; set; }
        public required ulong MessageId { get; set; }
        public required string LangCode { get; set; }
        public required TranslationResultForUser MessageContent { get; set; }
    }

}
