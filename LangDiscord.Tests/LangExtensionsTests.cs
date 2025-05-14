using LangDiscord.Enums;
using LangDiscord.Extensions;
using Xunit;

namespace LangDiscord.Tests
{
    public class LangExtensionsTests
    {
        [Theory]
        [InlineData(Lang.ENGLISH, "eng")]
        [InlineData(Lang.POLISH, "pol")]
        [InlineData(Lang.SPANISH, "spa")]
        public void GetLanguageCode_ReturnsCorrectCode(Lang lang, string expectedCode)
        {
            var code = lang.GetLanguageCode();
            Assert.Equal(expectedCode, code);
        }

        [Theory]
        [InlineData("eng", true)]
        [InlineData("pol", true)]
        [InlineData("spa", true)]
        [InlineData("zzz", false)]
        [InlineData("", false)]
        [InlineData(null, false)]
        public void IsValidLanguageCode_WorksCorrectly(string code, bool expectedResult)
        {
            var result = code?.IsValidLanguageCode() ?? false;
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void AllLanguagesWithCodes_ReturnsFormattedList()
        {
            var result = LangExtensions.AllLanguagesWithCodes;

            Assert.Contains("English - `eng`", result);
            Assert.Contains("Polish - `pol`", result);
            Assert.Contains("Spanish - `spa`", result);

            var lines = result.Split(Environment.NewLine);
            Assert.Equal(3, lines.Length);
        }
    }
}
